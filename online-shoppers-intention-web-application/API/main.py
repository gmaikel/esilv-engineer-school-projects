import pandas as pd
import streamlit as st
import joblib
from dataset_study_online_shoppers_intention.dataset_analysis_model import tab, plot_1, cmd1, tab_2

Choice = st.sidebar.selectbox("Pages", ("Prediction", "Dataset online_shoppers_intention"))
path = 'online_shoppers_intention.csv'

def client_caract():
    Administrative = st.sidebar.slider('Administrative',0,26)
    Administrative_Duration = st.sidebar.slider('Administrative_Duration', 0.0, 3398.750000, 0.0001)
    Informational = st.sidebar.slider('Informational',0,24)
    Informational_Duration = st.sidebar.slider('Informational_Duration', 0.0, 2549.375000, 0.0001)
    ProductRelated = st.sidebar.slider('ProductRelated',0,705)
    ProductRelated_Duration = st.sidebar.slider('ProductRelated_Duration', 0.0, 63973.522230, 0.0001)
    BounceRates = st.sidebar.slider('BounceRates', 0.0, 0.2000, 0.0001)
    ExitRates = st.sidebar.slider('ExitRates',0.0, 0.2000, 0.0001)
    PageValues = st.sidebar.slider('PageValues', 0.0, 361.763742, 0.0001)
    SpecialDay = st.sidebar.slider('SpecialDay', 0.0, 1.0, 0.0001)
    Month = st.sidebar.selectbox('Month',('Feb', 'Mar', 'May', 'Oct', 'June', 'Jul', 'Aug', 'Nov', 'Sep',
       'Dec'))
    OperatingSystems = st.sidebar.slider('OperatingSystems',0,8)
    Browser = st.sidebar.slider('Browser',1,13)
    Region = st.sidebar.slider('Region',1,9)
    TrafficType = st.sidebar.slider('TrafficType',1,20)
    VisitorType = st.sidebar.selectbox('VisitorType',('Returning_Visitor','New_Visitor','Other'))
    Weekend = st.sidebar.selectbox('Weekend',('True','False'))

    data={
    'Administrative' :Administrative,
    'Administrative_Duration' :Administrative_Duration,
    'Informational' :Informational,
    'Informational_Duration' :Informational_Duration,
    'ProductRelated' :ProductRelated,
    'ProductRelated_Duration' :ProductRelated_Duration,
    'BounceRates' :BounceRates,
    'ExitRates' :ExitRates,
    'PageValues' :PageValues,
    'SpecialDay' :SpecialDay,
    'Month' :Month,
    'OperatingSystems': OperatingSystems,
    'Browser' :Browser,
    'Region' :Region,
    'TrafficType' :TrafficType,
    'VisitorType' :VisitorType,
    'Weekend' :Weekend
    }

    profil_client = pd.DataFrame(data,index = [0])
    return profil_client

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

def main():
    global path
    if Choice == "Dataset online_shoppers_intention":
        st.title("Ameliorations : ")
        st.write("- Put all on the API to improve interaction with data")
        for i in tab:
            st.write(i)
        st.pyplot(plot_1[0])
        st.pyplot(plot_1[1])
        cmd1()

    else:
        st.sidebar.header("Les caractéristiques des personnes ")
        st.write(" #L'application qui prédit les achats ou non d'une personne")
        selection = st.sidebar.selectbox("Models", (
        "RandomForest_2(Acc 91%)", "RandomForest_1(Acc 88%)", "Gaussian Naive Bayes(Acc 85%) mode", "Extra Trees(90%) model"))
        input_df = client_caract()

        # transfo donne d'entree en donnée adapté au modèle
        data = pd.read_csv(path)

        if selection == "RandomForest_1(Acc 88%)":
            st.write("Using RandomForest_1(Acc 88%) model")
            path_model = "C:/Users/maikel/PycharmProjects/final-project-python/random_forest_technical_1.joblib"
        if selection == "RandomForest_2(Acc 91%)":
            st.write("Using RandomForest_2(Acc 91%) model")
            path_model = "C:/Users/maikel/PycharmProjects/final-project-python/RandomForest_technical_2.joblib"
        if selection == "Gaussian Naive Bayes":
            st.write("Using Gaussian Naive Bayes(Acc 85%) model")
            path_model = "C:/Users/maikel/PycharmProjects/final-project-python/GaussianNB_technical_2.joblib"
        if selection == "Extra Trees":
            st.write("Using Extra Trees(90%) model")
            path_model = "C:/Users/maikel/PycharmProjects/final-project-python/ExtraTreesClassifier_technical_2.joblib"

        # prendre premiere colonne
        st.subheader("Enter the data")
        st.write(input_df)
        st.subheader("Some caracteristics transform with data preprocessing  ")
        st.write(data)

        # load the model from disk
        loaded_model = joblib.load(path_model)

        if selection == "RandomForest_1(Acc 88%)":
            resultat = predict_RandomForest_28(loaded_model, input_df)
        else:
            resultat = predict(loaded_model, input_df)

        st.subheader('Prediction')
        st.write(resultat)
        st.write("True : The customer will buy")
        st.write("False : The customer will not buy")

if __name__ == '__main__':
    main()

