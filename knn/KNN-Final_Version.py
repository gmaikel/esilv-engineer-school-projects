# -*- coding: utf-8 -*-
"""
Created on Thu Apr 15 18:03:43 2021

@author: miko
"""
# %%

"""
Algorithme des k plus proches voisins
"""
# packages
import numpy as np
from random import shuffle
import matplotlib.pyplot as plt


# Data loader
donnees = []
constante_k = 3
donnees_apprentissage = []
donnees_test = []
donnees_validation = []
donnees_a_predire = []

#Chargement des données depuis le fichier data.csv
with open('D:\ESILV A3\Semestre 2\INFORMATIQUE\Datascience & IA\IA classification challenge/data.csv', 'r') as file:
    for ligne in file:
        a, b, c, d, e, f, classe = ligne.split(',')
        a, b, c, d, e, f = float(a), float(b), float(c), float(d), float(e), float(f)
        donnees.append([a, b, c, d, e, f, classe[:-1]])

#Chargement des données depuis le fichier preTest.csv
with open('D:\ESILV A3\Semestre 2\INFORMATIQUE\Datascience & IA\IA classification challenge/preTest.csv', 'r') as file:
    for ligne in file:
        a, b, c, d, e, f, classe = ligne.split(',')
        a, b, c, d, e, f = float(a), float(b), float(c), float(d), float(e), float(f)
        donnees.append([a, b, c, d, e, f, classe[:-1]])
        
#Chargement des données depuis le fichier finalTest.csv (données à prédire)
with open('D:\ESILV A3\Semestre 2\INFORMATIQUE\Datascience & IA\IA classification challenge/finalTest.csv', 'r') as file:
    for ligne in file:
        a, b, c, d, e, f = ligne.split(',')
        a, b, c, d, e, f = float(a), float(b), float(c), float(d), float(e), float(f)
        donnees_a_predire.append([a, b, c, d, e, f])


print(len(donnees), len(donnees_a_predire))
donnees_apprentissage = donnees[160:]
#donnees_test = donnees[:160]
#donnees_validation = donnees[1604:]
#print(len(donnees_a_predire))
#donnees_apprentissage = donnees


def distance(point_1, point_2):
    """
    Distance entre deux points
    point_1 = [a, b, c, d, e]
    point_2 = [a', b', c', d', e']
    distance = sqrt((a - a')**2 + ...)
    """
    return np.sqrt(sum((point_1[i] - point_2[i])**2 for i in range(len(point_1))))

def plus_commun(liste):
    return max(set(liste), key=liste.count)

def obtenir_k_voisins(point):
    """
        Une condition pour que l'algo s'adapte automatiquement aux données.
        Deux types de données :
            - Données de test (on a la classe)
            - Données à prédire (pas de classe)
    """
    if len(point) == 7:
        cle = lambda p: distance(p[:-1], point[:-1])
    else:
        cle = lambda p: distance(p[:-1], point)
    donnes_triees = sorted(donnees_apprentissage, key=cle)
    voisins = donnes_triees[:constante_k]
    return voisins

def predire(point):
    voisins = obtenir_k_voisins(point)
    classes = [v[-1] for v in voisins]
    return plus_commun(classes)
    
dictionnaire_classes = {
    'classA': 0,
    'classB': 1,
    'classC': 2,
    'classD': 3,
    'classE': 4,
}

def enregistrement_preediciton(liste):
    """
    Fonction qui permet de prédire les classes et les rengistrer dans un fichier txt
    """
    with open('D:\ESILV A3\Semestre 2\INFORMATIQUE\Datascience & IA\IA classification challenge/GALI_sample.txt', 'w') as file:
        for donnee in liste :
            prediction = predire(donnee)
            file.write(prediction+'\n')
    print("Fichier chargé correctement")


enregistrement_preediciton(donnees_a_predire)


"""
#Matrice de confusion pour les données d'apprentissage/test 
cpt =0
matrice_confusion = np.zeros((5, 5), dtype=int)
for donnee in donnees_test:
    prediction = predire(donnee)
    #print(prediction)
    verite = donnee[-1]
    #print(verite)
    i = dictionnaire_classes[prediction]
    j = dictionnaire_classes[verite]
    matrice_confusion[i][j] += 1
    if prediction != verite :
        cpt +=1
    

print(matrice_confusion)
print('\nLe taux de précision de l algorithme : ', (1-(cpt/len(donnees_test)))*100,'%')


#Choix de la valeur de k
erreurs = []
for constante_k in range(1, 100):
    nombre_erreurs = 0
    for donnee in donnees_test:
        prediction = predire(donnee)
        verite = donnee[-1]
        if prediction != verite:
            nombre_erreurs += 1
    erreurs.append(nombre_erreurs)

plt.plot(range(1, 100), erreurs)
plt.title('Nombre d erreur par rapport à K')
plt.xlabel('K')
plt.ylabel('Nombre d erreur')
plt.show()
print("FINIT")
"""
