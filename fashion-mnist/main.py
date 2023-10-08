# importation des packages

# Bibliothèques pour le modèle
from tensorflow.keras.layers import Dense, Dropout, Flatten, Activation, Conv2D, MaxPooling2D
from tensorflow.keras.optimizers import Adam
from tensorflow.keras.models import Sequential
import tensorflow as tf

# Bibliothèques pour la matrice de confusion
from sklearn.metrics import confusion_matrix
import seaborn as sn;sn.set(font_scale=1.4)

# Bibliothèques générales
import matplotlib.pyplot as plt
import matplotlib.image as img
from tqdm import tqdm
import pandas as pd
import numpy as np
import os

classes = [i for i in range(10)]
indice_classes = {0: 'T-shirt/top', 1: 'Trowser', 2: 'Pullover', 3: 'Dress', 4: 'Coat', 5: 'Sandal', 6: 'Shirt',
                      7: 'Sneaker', 8: 'Bag', 9: 'Ankle Boot'}

# Visualisaiton des résultats

def plot_accuracy_loss(history):
    """
        Plot accuracy/loss, objectif :  constater l'overfitting / underfitting.
    """
    fig = plt.figure(figsize=(10,5))

    # Plot accuracy
    plt.subplot(221)
    plt.plot(history.history['accuracy'],'bo--', label = "acc")
    plt.plot(history.history['val_accuracy'], 'ro--', label = "val_acc")
    plt.title("train accuracy vs val_accuracy")
    plt.ylabel("accuracy")
    plt.xlabel("epochs")
    plt.legend()

    # Plot loss
    plt.subplot(222)
    plt.plot(history.history['loss'],'bo--', label = "loss")
    plt.plot(history.history['val_loss'], 'ro--', label = "val_loss")
    plt.title("train_loss vs val_loss")
    plt.ylabel("loss")
    plt.xlabel("epochs")

    plt.legend()
    plt.show()
    print('\n')


def exemples_images(dataset, dataset_indices):
    """
       Cette focntion permet d'afficher 25 images, sera utilisée pour afficher les images mal classifiées.
    """

    fig = plt.figure(figsize=(10, 10))
    fig.suptitle("Voici un exemple de 25 images", fontsize=15)
    for i in range(25):
        plt.subplot(5, 5, i + 1)
        plt.xticks([])
        plt.yticks([])
        plt.grid(False)
        plt.imshow(dataset[i], cmap=plt.cm.binary)
        plt.xlabel(classes[dataset_indices[i]])
    plt.show()


def images_mal_classifiees(test_images, test_labels, pred_labels):
    """
        Cette fonction va nous renvoyer un échantillon de 25 images mal classifiées ==> test_labels != pred_labels
    """
    BOO = (test_labels == pred_labels)
    index = np.where(BOO == 0)
    malclass_images = test_images[index]
    malclass_indice = pred_labels[index]

    title = "Exemples d'images mal classifiées:"
    exemples_images(malclass_images, malclass_indice)


def print_hi():
    verification_GPU = tf.config.experimental.list_physical_devices('GPU')
    print("GPU ok" if len(verification_GPU) == 1 else "Pas de GPU")

    # Data loader
    from tensorflow.keras.datasets import fashion_mnist
    print("Start")
    (train_images, train_indices), (val_images, val_indices) = fashion_mnist.load_data()
    print("En cours 1")
    taille_image = (128, 128)
    import numpy as np
    import cv2
    images = []
    for i in range(len(train_images)):
        image = cv2.cvtColor(train_images[i], cv2.COLOR_BGR2RGB)
        image = cv2.resize(image, taille_image)
        images.append(image)

    images_t = np.array(images)
    images = []
    for i in range(len(val_images)):
        image = cv2.cvtColor(train_images[i], cv2.COLOR_BGR2RGB)
        image = cv2.resize(image, taille_image)
        images.append(image)

    images_v = np.array(images)



    images_t = images_t / 255.0
    images_v = images_v / 255.0
    print("En cours")
    # Création du modèle CNN

    model = Sequential()

    # 1ere couche (convolution & polling)
    model.add(Conv2D(32, (3, 3), activation='relu', input_shape=(128, 128, 3)))
    model.add(MaxPooling2D(pool_size=(2, 2)))

    # 2eme couche (convolution & polling)
    model.add(Conv2D(64, (3, 3), activation='relu'))
    model.add(MaxPooling2D(pool_size=(2, 2)))

    ## 3eme couche (convolution & polling)
    model.add(Conv2D(64, (3, 3), activation='relu'))
    model.add(MaxPooling2D(pool_size=(2, 2)))
    model.add(Dropout(0.3))

    # 4eme couche (convolution & polling)
    model.add(Conv2D(64, (3, 3), activation='relu'))
    model.add(MaxPooling2D(pool_size=(2, 2)))

    # Construction d'un vecteur à partir de la matrice
    model.add(Flatten())

    # Deux couches dense
    model.add(Dense(64, activation="relu"))
    model.add(Dropout(0.2))
    model.add(Dense(10,activation="softmax"))  # "softmax" pour l'activation, pour avoir la probabilité entre nos 10 classes

    # Compilation

    model.compile(optimizer=Adam(learning_rate=0.0005), loss='sparse_categorical_crossentropy', metrics=['accuracy'])

    # Entraintement du modèle

    history = model.fit(images_t, train_indices, batch_size=32, epochs=10, validation_split=0.2)

    test_modele = model.evaluate(images_v, val_indices)
    print("\nLa précision du modèle : ", round(test_modele[1], 2), "\nlearning_rate : 0.0005", "\nbatch_size : 32",
          "\nepochs : 10")

    plot_accuracy_loss(history)

    predictions = model.predict(val_images)
    pred_indices = np.argmax(predictions, axis=1)

    CM = confusion_matrix(val_indices, pred_indices)
    ax = plt.axes()
    sn.heatmap(CM, annot=True,
               annot_kws={"size": 15},
               xticklabels=classes,
               yticklabels=classes, ax=ax)
    ax.set_title('Matrice de confusion')
    plt.show()

    images_mal_classifiees(val_images, val_indices, pred_indices)


# Press the green button in the gutter to run the script.
if __name__ == '__main__':
    print_hi()

# See PyCharm help at https://www.jetbrains.com/help/pycharm/
