from sklearn.tree import DecisionTreeClassifier
from sklearn.pipeline import make_pipeline
from sklearn.neighbors import KNeighborsClassifier
from sklearn.naive_bayes import GaussianNB
from sklearn.linear_model import LogisticRegression
from sklearn.naive_bayes import BernoulliNB
from sklearn import metrics
from sklearn.dummy import DummyClassifier
from sklearn.cluster import KMeans
from sklearn.metrics import log_loss, classification_report, confusion_matrix, roc_auc_score
from sklearn.model_selection import *
from sklearn.svm import *
from sklearn.ensemble import *
from sklearn.pipeline import Pipeline
from sklearn.preprocessing import StandardScaler, LabelEncoder
from sklearn.inspection import permutation_importance

import matplotlib.pyplot as plt

import numpy as np
import pandas as pd
import math
import warnings
import seaborn as sns
import scipy.stats as stats
import eli5

import joblib

plt.rcParams['figure.figsize'] = [12,6]
warnings.filterwarnings('ignore')

path = "online_shoppers_intention.csv"
df1 = pd.read_csv(path)
df1.dataframeName = 'online_shoppers_intention.csv'

#Functions ----------------------
# Scatter and density plots
def plotScatterMatrix(df, plotSize, textSize):
    df = df.select_dtypes(include=[np.number])  # keep only numerical columns
    # Remove rows and columns that would lead to df being singular
    df = df.dropna('columns')
    df = df[[col for col in df if df[col].nunique() > 1]]  # keep columns where there are more than 1 unique values
    columnNames = list(df)
    if len(columnNames) > 10:  # reduce the number of columns for matrix inversion of kernel density plots
        columnNames = columnNames[:10]
    df = df[columnNames]
    ax = pd.plotting.scatter_matrix(df, alpha=0.75, figsize=[plotSize, plotSize], diagonal='kde')
    corrs = df.corr().values
    for i, j in zip(*plt.np.triu_indices_from(ax, k=1)):
        ax[i, j].annotate('Corr. coef = %.3f' % corrs[i, j], (0.8, 0.2), xycoords='axes fraction', ha='center',
                          va='center', size=textSize)
    plt.suptitle('Scatter and Density Plot')
    plt.show()

# Clustering
def find_clusters(X, start=1, end=11):
    """
    data: array like
    """
    inertias = []

    for i in range(start, end):
        kmeans = KMeans(n_clusters=i)
        kmeans.fit(X)
        inertias.append(kmeans.inertia_)

    plt.plot(np.arange(start, end), np.array(inertias), color='orange', marker='o')
    plt.xlabel('No. of clusters')
    plt.ylabel('Inertia')
    plt.title("Elbow Curve", fontsize=16)
    plt.show()

def draw_optimal_clusters(X, n_clusters=2, xlabel=None, ylabel=None, title=None):
    kmeans = KMeans(n_clusters=n_clusters)
    kmeans.fit(X)

    y_means = kmeans.predict(X)
    centers = kmeans.cluster_centers_

    plt.scatter(X[y_means == 0, 0], X[y_means == 0, 1], color='lightblue', label='Uninterested Customers')
    plt.scatter(X[y_means == 1, 0], X[y_means == 1, 1], color='orange', label='Interested Customers')
    plt.scatter(centers[:, 0], centers[:, 1], s=100, label='Centers')

    plt.legend()

    plt.title(title, fontsize=20)
    plt.xlabel(xlabel)
    plt.ylabel(ylabel)
    plt.show()

def predict_RandomForest_28(model, X):
  """
  For RandomForest_28 (28 variables)
  """
  d = pd.DataFrame(X)
  df = d
  d_ = pd.get_dummies(df)
  col = ['Month_Aug', 'Month_Dec', 'Month_Feb', 'Month_Jul', 'Month_June',
        'Month_Mar', 'Month_May', 'Month_Nov', 'Month_Oct', 'Month_Sep',
        'VisitorType_New_Visitor', 'VisitorType_Other',
        'VisitorType_Returning_Visitor']
  df = d_.reindex(d_.columns.union(col, sort=False), axis=1, fill_value=0)
  return model.predict(df)[0]

def predict(model, X):
  """
  For other model exept RandomForest_28 (28 variables)
  """
  d = pd.DataFrame(X)
  shopping_clean = d.drop(['Month', 'Browser', 'OperatingSystems', 'Region', 'TrafficType', 'Weekend'], axis=1)
  # Encoding Vistor Type
  visitor_encoded = pd.get_dummies(shopping_clean['VisitorType'], prefix='Visitor_Type', drop_first=True)
  d_ = pd.concat([shopping_clean, visitor_encoded], axis=1).drop(['VisitorType'], axis=1)
  col = ['Visitor_Type_Other', 'Visitor_Type_Returning_Visitor']
  d_ = d_.reindex(d_.columns.union(col, sort=False), axis=1, fill_value=0)
  return model.predict(d_)[0]
# -----------------------------------
# Functions analysis ----------------
#Plots général - ok
def plotting_data_analysis_other_2():
    # DATA Vizualization
    df = pd.read_csv(path)
    fig, ax = plt.subplots(4, 2, figsize=(14, 14))
    fig.tight_layout(pad=4)
    sns.countplot(df['SpecialDay'], palette='dark', ax=ax[0, 0])
    sns.countplot(df['Month'], palette='dark', ax=ax[0, 1])
    sns.countplot(df['OperatingSystems'], palette='dark', ax=ax[1, 0])
    sns.countplot(df['Browser'], palette='dark', ax=ax[1, 1])
    sns.countplot(df['Region'], palette='dark', ax=ax[2, 0])
    sns.countplot(df['TrafficType'], palette='dark', ax=ax[2, 1])
    sns.countplot(df['VisitorType'], palette='dark', ax=ax[3, 0])
    sns.countplot(df['Weekend'], palette='dark', ax=ax[3, 1])
    # Matrice de confusion
    plt.figure(figsize=(18, 14))
    sns.heatmap(df.corr(), cmap='Blues', annot=True)
    return fig, plt

#Data analysis
df1 = pd.read_csv(path)
#Manuel analysis - ok
def a():
    # Plotting visitors
    df1['VisitorType'].value_counts()
    # plotting a pie chart for browsers
    plt.rcParams['figure.figsize'] = (18, 7)
    size = [10551, 1694, 85]
    colors = ['violet', 'magenta', 'pink']
    labels = "Returning Visitor", "New_Visitor", "Others"
    explode = [0, 0, 0.1]
    plt.subplot(1, 2, 1)
    plt.pie(size, colors=colors, labels=labels, explode=explode, shadow=True, autopct='%.2f%%')
    plt.title('Different Visitors', fontsize=30)
    plt.axis('off')
    plt.legend()
    return plt
def b():
    # plotting a pie chart for browsers
    size = [7961, 2462, 736, 467, 174, 163, 300]
    colors = ['orange', 'yellow', 'pink', 'crimson', 'lightgreen', 'cyan', 'blue']
    labels = "2", "1", "4", "5", "6", "10", "others"
    plt.subplot(1, 2, 2)
    plt.pie(size, colors=colors, labels=labels, shadow=True, autopct='%.2f%%', startangle=90)
    plt.title('Different Browsers', fontsize=30)
    plt.axis('off')
    plt.legend()
    return plt
#Revenue - ok
def c():

    # product related duration vs revenue
    plt.rcParams['figure.figsize'] = (18, 15)
    plt.subplot(2, 2, 1)
    sns.boxenplot(df1['Revenue'], df1['Informational_Duration'], palette='rainbow')
    plt.title('Info. duration vs Revenue', fontsize=30)
    plt.xlabel('Info. duration', fontsize=15)
    plt.ylabel('Revenue', fontsize=15)
    return plt
def d():
    # product related duration vs revenue
    plt.subplot(2, 2, 2)
    sns.boxenplot(df1['Revenue'], df1['Administrative_Duration'], palette='pastel')
    plt.title('Admn. duration vs Revenue', fontsize=30)
    plt.xlabel('Admn. duration', fontsize=15)
    plt.ylabel('Revenue', fontsize=15)
    return plt
def e():
    # product related duration vs revenue
    plt.subplot(2, 2, 3)
    sns.boxenplot(df1['Revenue'], df1['ProductRelated_Duration'], palette='dark')
    plt.title('Product Related duration vs Revenue', fontsize=30)
    plt.xlabel('Product Related duration', fontsize=15)
    plt.ylabel('Revenue', fontsize=15)
    return plt
def f():
    # exit rate vs revenue
    plt.subplot(2, 2, 4)
    sns.boxenplot(df1['Revenue'], df1['ExitRates'], palette='spring')
    plt.title('ExitRates vs Revenue', fontsize=30)
    plt.xlabel('ExitRates', fontsize=15)
    plt.ylabel('Revenue', fontsize=15)
    return plt
def g():
    # page values vs revenue
    plt.rcParams['figure.figsize'] = (18, 7)
    plt.subplot(1, 2, 1)
    sns.stripplot(df1['Revenue'], df1['PageValues'], palette='autumn')
    plt.title('PageValues vs Revenue', fontsize=30)
    plt.xlabel('PageValues', fontsize=15)
    plt.ylabel('Revenue', fontsize=15)
    return plt
def h():
    # bounce rates vs revenue
    plt.subplot(1, 2, 2)
    sns.stripplot(df1['Revenue'], df1['BounceRates'], palette='magma')
    plt.title('Bounce Rates vs Revenue', fontsize=30)
    plt.xlabel('Boune Rates', fontsize=15)
    plt.ylabel('Revenue', fontsize=15)
    return plt
def i():
    # month vs pagevalues wrt revenue
    plt.rcParams['figure.figsize'] = (18, 15)
    plt.subplot(2, 2, 1)
    sns.boxplot(x=df1['Month'], y=df1['PageValues'], hue=df1['Revenue'], palette='inferno')
    plt.title('Mon. vs PageValues w.r.t. Rev.', fontsize=30)
    return plt
def j():
    # month vs exitrates wrt revenue
    plt.subplot(2, 2, 2)
    sns.boxplot(x=df1['Month'], y=df1['ExitRates'], hue=df1['Revenue'], palette='Reds')
    plt.title('Mon. vs ExitRates w.r.t. Rev.', fontsize=30)
    return plt
def k():
    # month vs bouncerates wrt revenue
    plt.subplot(2, 2, 3)
    sns.boxplot(x=df1['Month'], y=df1['BounceRates'], hue=df1['Revenue'], palette='Oranges')
    plt.title('Mon. vs BounceRates w.r.t. Rev.', fontsize=30)
    return plt
def l():
    # visitor type vs exit rates w.r.t revenue
    plt.subplot(2, 2, 4)
    sns.boxplot(x=df1['VisitorType'], y=df1['BounceRates'], hue=df1['Revenue'], palette='Purples')
    plt.title('Visitors vs BounceRates w.r.t. Rev.', fontsize=30)
    return plt
def m():
    # visitor type vs exit rates w.r.t revenue
    plt.rcParams['figure.figsize'] = (18, 15)
    plt.subplot(2, 2, 1)
    sns.violinplot(x=df1['VisitorType'], y=df1['ExitRates'], hue=df1['Revenue'], palette='rainbow')
    plt.title('Visitors vs ExitRates wrt Rev.', fontsize=30)
    return plt
def n():
    # visitor type vs exit rates w.r.t revenue
    plt.subplot(2, 2, 2)
    sns.violinplot(x=df1['VisitorType'], y=df1['PageValues'], hue=df1['Revenue'], palette='gnuplot')
    plt.title('Visitors vs PageValues wrt Rev.', fontsize=30)
    return plt
def o():
    # region vs pagevalues w.r.t. revenue
    plt.subplot(2, 2, 3)
    sns.violinplot(x=df1['Region'], y=df1['PageValues'], hue=df1['Revenue'], palette='Greens')
    plt.title('Region vs PageValues wrt Rev.', fontsize=30)
    return plt
def p():
    # region vs exit rates w.r.t. revenue
    plt.subplot(2, 2, 4)
    sns.violinplot(x=df1['Region'], y=df1['ExitRates'], hue=df1['Revenue'], palette='spring')
    plt.title('Region vs Exit Rates w.r.t. Revenue', fontsize=30)
    return plt
#ok
def Other_plotting_other_variables():
    continuous_features = ['Administrative_Duration',
                           'Informational_Duration',
                           'ProductRelated_Duration',
                           'BounceRates',
                           'ExitRates',
                           'PageValues']
    discrete_features = ['Administrative', 'Informational',
                         'ProductRelated', 'Month',
                         'OperatingSystems', 'Browser',
                         'Region', 'TrafficType',
                         'VisitorType', 'Weekend']
    chi_squared_df = pd.DataFrame(columns=['feature', 'pval', 'dependent'])
    for i, col in enumerate(discrete_features):
        x = df1.groupby(['Revenue', col])[col].count().unstack(1).fillna(0).astype('int')
        res = stats.chi2_contingency(x.values)
        pval = res[1]
        if pval < 0.05:
            dependent = 'Yes'
        else:
            dependent = 'No'
        chi_squared_df.loc[i] = [col, round(pval, 3), dependent]
    chi_squared_df

    continuous_features
    fig, axs = plt.subplots(nrows=2, ncols=3, figsize=(15, 20))
    axs = np.ravel(axs)
    for i, col in enumerate(continuous_features):
        plt.sca(axs[i])
        sns.violinplot(data=df1, x='Revenue', y=col)
    plt.tight_layout()
    plt.show()
    print()
#ok
def plotting_data_analysis_revenue_false_true():
    # Data vizualisation
    customer = pd.read_csv(path)
    customer_copy = customer.copy()
    plt.figure(figsize=(15, 5))
    # plt.style.use('seaborn-white')
    plt.subplot(131)
    sns.scatterplot(x="Administrative", y="Administrative_Duration", hue="Revenue", data=customer_copy)
    plt.subplot(132)
    sns.scatterplot(x="Informational", y="Informational_Duration", hue="Revenue", data=customer_copy)
    plt.subplot(133)
    sns.scatterplot(x="ProductRelated", y="ProductRelated_Duration", hue="Revenue", data=customer_copy)
    plt.show()
    print()

    sns.relplot(x="BounceRates", y="ExitRates", col="Revenue", hue="Revenue", style="Weekend", data=customer_copy)
    plt.show()
    print()

    sns.catplot(x="VisitorType", y="ExitRates",
                hue="Weekend", col="Revenue",
                data=customer_copy, kind="box");
    plt.show()
    print()


#Linear regression and Hear Map - ok
def plotting_LR_HeatMap():
    # Bounce Rate vs Exit Rate
    sns.lmplot(x="BounceRates", y="ExitRates", data=df1,
               scatter_kws={'alpha': 0.3},
               line_kws={'color': 'red', 'ls': '--'})
    plt.show()

    # Page Value versus Bounce Rate
    sns.lmplot(x="PageValues", y="BounceRates",
               data=df1, scatter_kws={'alpha': 0.2},
               line_kws={'color': 'red', 'ls': '--'})
    plt.show()

    # Page Value versus Exit Rate
    sns.lmplot(x="PageValues", y="ExitRates",
               data=df1, scatter_kws={'alpha': 0.2},
               line_kws={'color': 'red', 'ls': '--'})
    plt.show()

    # Impact of Administrative Page Views and Administrative Pageview Duration on Revenue
    sns.lmplot(x="Administrative", y="Administrative_Duration",
               hue='Revenue', data=df1, scatter_kws={'alpha': 0.5})
    plt.show()

    # Impact of Information Page Views and Information Pageview Duration on Revenue
    sns.lmplot(x="Informational", y="Informational_Duration",
               hue='Revenue', data=df1, scatter_kws={'alpha': 0.5})
    plt.show()

    # Impact of ProductRelated Page Views and ProductRelated Pageview Duration on Revenue
    sns.lmplot(x="ProductRelated", y="ProductRelated_Duration",
               hue='Revenue', data=df1, scatter_kws={'alpha': 0.5})
    plt.show()

    google_analytics_features = ['BounceRates', 'ExitRates', 'PageValues']
    corr_mat = df1[google_analytics_features].corr()
    sns.heatmap(corr_mat, annot=True, annot_kws={'fontsize': 16}, fmt='.3g', linewidths=2, cmap='Pastel1')
    plt.show()

    type_of_pages = ['Administrative', 'Informational', 'ProductRelated']
    corr_mat = df1[type_of_pages].corr(method='spearman')
    sns.heatmap(corr_mat, annot=True, annot_kws={'fontsize': 16}, fmt='.3g', linewidths=2, cmap='Pastel1')
    plt.show()

    # lm plot
    plt.rcParams['figure.figsize'] = (20, 10)
    sns.lmplot(x='Administrative', y='Informational', data=df1, x_jitter=0.05)
    plt.title('LM Plot between Admistrative and Information', fontsize=15)
    plt.show()

#K-means - ok
def K_Means():
    # Performing K-means Clustering for Informational Duration versus Bounce Rate
    X = df1.iloc[:, [3, 6]].values
    find_clusters(X)
    draw_optimal_clusters(X, n_clusters=2, xlabel="Informational Duration",
                          ylabel="Bounce Rates",
                          title="Informational Duration vs Bounce Rates")

    # K-means Clustering for Informational Duration versus Exit Rate
    X = df1.iloc[:, [4, 7]].values
    find_clusters(X)
    draw_optimal_clusters(X, n_clusters=2, xlabel='Exit Rates',
                          ylabel='Informational Duration',
                          title='Informational Duration vs Exit Rates')

    # K-means Clustering for Administrative Duration versus Bounce Rate
    X = df1.iloc[:, [1, 6]].values
    find_clusters(X)
    draw_optimal_clusters(X, n_clusters=2, xlabel="Administrative Duration",
                          ylabel="Bounce Rates",
                          title="Administrative Duration vs Bounce Rates")

    X = df1.iloc[:, [1, 7]].values
    find_clusters(X)
    draw_optimal_clusters(X, n_clusters=2, xlabel="Administrative Duration",
                          ylabel="Exit Rate",
                          title="Administrative Duration Vs Exit Rate")

    x = df1.iloc[:, [3, 6]].values
    km = KMeans(n_clusters=3, init='k-means++', max_iter=300, n_init=10, random_state=0)
    y_means = km.fit_predict(x)
    plt.scatter(x[y_means == 0, 0], x[y_means == 0, 1], s=100, c='pink', label='Un-interested Customers')
    plt.scatter(x[y_means == 1, 0], x[y_means == 1, 1], s=100, c='yellow', label='General Customers')
    plt.scatter(x[y_means == 2, 0], x[y_means == 2, 1], s=100, c='cyan', label='Target Customers')
    plt.scatter(km.cluster_centers_[:, 0], km.cluster_centers_[:, 1], s=50, c='blue', label='centeroid')
    plt.title('Administrative Duration vs Duration', fontsize=20)
    plt.grid()
    plt.xlabel('Administrative Duration')
    plt.ylabel('Bounce Rates')
    plt.legend()
    plt.show()

    x = df1.iloc[:, [1, 7]].values
    km = KMeans(n_clusters=2, init='k-means++', max_iter=300, n_init=10, random_state=0)
    y_means = km.fit_predict(x)
    plt.scatter(x[y_means == 0, 0], x[y_means == 0, 1], s=100, c='pink', label='Un-interested Customers')
    plt.scatter(x[y_means == 1, 0], x[y_means == 1, 1], s=100, c='yellow', label='Target Customers')
    plt.scatter(km.cluster_centers_[:, 0], km.cluster_centers_[:, 1], s=50, c='blue', label='centeroid')
    plt.title('Administrative Clustering vs Exit Rates', fontsize=20)
    plt.grid()
    plt.xlabel('Administrative Duration')
    plt.ylabel('Exit Rates')
    plt.legend()
    plt.show()

    # informational duration vs Bounce Rates
    x = df1.iloc[:, [13, 14]].values
    km = KMeans(n_clusters=2, init='k-means++', max_iter=300, n_init=10, random_state=0)
    y_means = km.fit_predict(x)
    plt.scatter(x[y_means == 0, 0], x[y_means == 0, 1], s=100, c='pink', label='Un-interested Customers')
    plt.scatter(x[y_means == 1, 0], x[y_means == 1, 1], s=100, c='yellow', label='Target Customers')
    plt.scatter(km.cluster_centers_[:, 0], km.cluster_centers_[:, 1], s=50, c='blue', label='centeroid')
    plt.title('Region vs Traffic Type', fontsize=20)
    plt.grid()
    plt.xlabel('Region')
    plt.ylabel('Traffic')
    plt.legend()
    plt.show()

    # informational duration vs Bounce Rates
    x = df1.iloc[:, [1, 13]].values
    km = KMeans(n_clusters=2, init='k-means++', max_iter=300, n_init=10, random_state=0)
    y_means = km.fit_predict(x)
    plt.scatter(x[y_means == 0, 0], x[y_means == 0, 1], s=100, c='pink', label='Unproductive Customers')
    plt.scatter(x[y_means == 1, 0], x[y_means == 1, 1], s=100, c='yellow', label='Target Customers')
    plt.scatter(km.cluster_centers_[:, 0], km.cluster_centers_[:, 1], s=50, c='blue', label='centeroid')
    plt.title('Adminstrative Duration vs Region', fontsize=20)
    plt.grid()
    plt.xlabel('Administrative Duration')
    plt.ylabel('Region Type')
    plt.legend()
    plt.show()
# ----------------------------------
# Analysis
 #Analyses des données général
df1 = pd.read_csv(path)
nRow, nCol = df1.shape
alpha = f"There are {nRow} rows and {nCol} columns"
plot_1 = plotting_data_analysis_other_2()
gamma = "Total number of duplicate rows: ", df1.duplicated().sum()
delta = "Sample of Data :"
iei = df1.sample(5)
sos = "Description of data :"
sita = df1.describe().T
#
print("Convert Colmuns(OperatingSystems, Browser, Region, TrafficType) to String..")
df1.OperatingSystems = df1.OperatingSystems.astype(str)
df1.Browser = df1.Browser.astype(str)
df1.Region = df1.Region.astype(str)
df1.TrafficType = df1.TrafficType.astype(str)
print("Columns converted to string")
print()

def cmd1():
    df1.OperatingSystems = df1.OperatingSystems.astype(str)
    df1.Browser = df1.Browser.astype(str)
    df1.Region = df1.Region.astype(str)
    df1.TrafficType = df1.TrafficType.astype(str)
tab = [alpha, gamma, delta, iei, sos, sita]
# #Fin d'analyse des données général

tab_2 = [a(), b(), c(), d(), e(), f(), g(), h(), i(), j(), k(), l(), m(), (n), o(), p()]




description_t = pd.read_csv(path).describe().T


