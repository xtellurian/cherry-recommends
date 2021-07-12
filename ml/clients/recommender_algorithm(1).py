import json
import random
import pickle
import pandas as pd
import numpy as np


def init():
    print("This is init")

pills_cap_potential_list = [2,3,4,5,6,8,10]
bulk_cap_potential_list = [0,1,2]
basket_value_cap_potential_list = [40,50,60,70,80,90,100,120,140,160,200,250,300,400]    
pills_cap_potential_probs = [0.05,0.05,0.05,0.05,0.05,0.05,0.7]
bulk_cap_potential_probs = [0.1,0.1,0.8]
basket_value_cap_potential_probs = [0.025,0.025,0.025,0.025,0.025,0.025,0.675,0.025,0.025,0.025,0.025,0.025,0.025,0.025]   




############ SEGMENT CUSTOMERS ON THEIR QUIZ RESPONSES
def segment_quiz_response(test):
    
    with open('source_files/model.pkl', 'rb') as file: 
        my_model = pickle.load(file)

    segment_label = "U"
    q_age = test['q_age']
    q_alcoholic_drink_a_night = test['q_alcoholic_drink_a_night']
    q_allergy = test['q_allergy']
    q_antibiotics = test['q_antibiotics']
    q_bones = test['q_bones']
    q_brain_fog = test['q_brain_fog']
    q_condition = test['q_condition']
    q_constipated = test['q_constipated']
    q_currently_take = test['q_currently_take']
    q_dairy = test['q_dairy']
    q_describe_diets = test['q_describe_diets']
    q_diarrhea = test['q_diarrhea']
    q_digestive_health = test['q_digestive_health']
    q_eat_protein = test['q_eat_protein']
    q_energy_level = test['q_energy_level']
    q_energy_slumps_experience = test['q_energy_slumps_experience']
    q_exercise_before_bed = test['q_exercise_before_bed']
    q_exercise_days = test['q_exercise_days']
    q_exercise_intensity_rating = test['q_exercise_intensity_rating']
    q_exercise_type = test['q_exercise_type']
    q_feel_stressed = test['q_feel_stressed']
    q_feel_unwell = test['q_feel_unwell']
    q_feeling_sluggish = test['q_feeling_sluggish']
    q_fish = test['q_fish']
    q_fitness_focus = test['q_fitness_focus']
    q_focusing = test['q_focusing']
    q_fruit_vegetables = test['q_fruit_vegetables']
    q_gallstone = test['q_gallstone']
    q_gender = test['q_gender']
    q_hair = test['q_hair']
    q_heart_concern = test['q_heart_concern']
    q_history_of_heart_problems = test['q_history_of_heart_problems']
    q_improve_area = test['q_improve_area']
    q_iron = test['q_iron']
    q_joints = test['q_joints']
    q_meat = test['q_meat']
    q_medications_taking = test['q_medications_taking']
    q_memory = test['q_memory']
    q_muscle_cramps = test['q_muscle_cramps']
    q_natal = test['q_natal']
    q_often_get_cold = test['q_often_get_cold']
    restriction = test['restriction']
    q_skin = test['q_skin']
    q_sleep_screen = test['q_sleep_screen']
    q_smoke = test['q_smoke']
    q_stress_level = test['q_stress_level']
    q_sugar = test['q_sugar']
    q_sunshine = test['q_sunshine']
    q_supplement_perception = test['q_supplement_perception']
    q_take_prescription	 = test['q_take_prescription']
    q_taken_in_the_past = test['q_taken_in_the_past']
    q_trying_form_baby = test['q_trying_form_baby']
    q_urinary_tract_health	 = test['q_urinary_tract_health']
    immunity_goal = test['immunity_goal']
    energy_goal = test['energy_goal']
    digestion_goal = test['digestion_goal']
    sleep_goal = test['sleep_goal']
    skin_goal = test['skin_goal']
    brain_goal = test['brain_goal']
    heart_goal = test['heart_goal']
    hair_goal = test['hair_goal']
    joints_goal = test['joints_goal']
    bones_goal = test['bones_goal']
    stress_goal = test['stress_goal']
    number_goals = test['number_goals']


    quiz_responses = {
        "q_age": [q_age],
        "q_alcoholic_drink_a_night": [q_alcoholic_drink_a_night],
        "q_allergy": [q_allergy],
        "q_antibiotics": [q_antibiotics],
        "q_bones": [q_bones],
        "q_brain_fog": [q_brain_fog],
        "q_condition": [q_condition],
        "q_constipated": [q_constipated],
        "q_currently_take": [q_currently_take],
        "q_dairy": [q_dairy],
        "q_describe_diets": [q_describe_diets],
        "q_diarrhea": [q_diarrhea],
        "q_digestive_health": [q_digestive_health],
        "q_eat_protein": [q_eat_protein],
        "q_energy_level": [q_energy_level],
        "q_energy_slumps_experience": [q_energy_slumps_experience],
        "q_exercise_before_bed": [q_exercise_before_bed],
        "q_exercise_days": [q_exercise_days],
        "q_exercise_intensity_rating": [q_exercise_intensity_rating],
        "q_exercise_type": [q_exercise_type],
        "q_feel_stressed": [q_feel_stressed],
        "q_feel_unwell": [q_feel_unwell],
        "q_feeling_sluggish": [q_feeling_sluggish],
        "q_fish": [q_fish],
        "q_fitness_focus": [q_fitness_focus],
        "q_focusing": [q_focusing],
        "q_fruit_vegetables": [q_fruit_vegetables],
        "q_gallstone": [q_gallstone],
        "q_gender": [q_gender],
        "q_hair": [q_hair],
        "q_heart_concern": [q_heart_concern],
        "q_history_of_heart_problems": [q_history_of_heart_problems],
        "q_improve_area": [q_improve_area],
        "q_iron": [q_iron],
        "q_joints": [q_joints],
        "q_meat": [q_meat],
        "q_medications_taking": [q_medications_taking],
        "q_memory": [q_memory],
        "q_muscle_cramps": [q_muscle_cramps],
        "q_natal": [q_natal],
        "q_often_get_cold": [q_often_get_cold],
        "restriction": [restriction],
        "q_skin": [q_skin],
        "q_sleep_screen": [q_sleep_screen],
        "q_smoke": [q_smoke],
        "q_stress_level": [q_stress_level],
        "q_sugar": [q_sugar],
        "q_sunshine": [q_sunshine],
        "q_supplement_perception": [q_supplement_perception],
        "q_take_prescription": [q_take_prescription],
        "q_taken_in_the_past": [q_taken_in_the_past],
        "q_trying_form_baby": [q_trying_form_baby],
        "q_urinary_tract_health": [q_urinary_tract_health],
        "immunity_goal": [immunity_goal],
        "energy_goal": [energy_goal],
        "digestion_goal": [digestion_goal],
        "sleep_goal": [sleep_goal],
        "skin_goal": [skin_goal],
        "brain_goal": [brain_goal],
        "heart_goal": [heart_goal],
        "hair_goal": [hair_goal],
        "joints_goal": [joints_goal],
        "bones_goal": [bones_goal],
        "stress_goal": [stress_goal],
        "number_goals": [number_goals]

    }

    df = pd.DataFrame(quiz_responses)
    segment_label = my_model.predict(df)

    return segment_label


######### GET PARAMETERS FOR EACH SEGMENT
def get_parameters_for_segment(segment_label):

    recommendedParameters = {
        "pills_cap": 10,
        "bulk_cap": 2,
        "basket_value_cap": 100
    }
    
    if segment_label == 'AA':
        recommendedParameters["pills_cap"] = int(np.random.choice(pills_cap_potential_list, p = pills_cap_potential_probs))
        recommendedParameters["bulk_cap"] = int(np.random.choice(bulk_cap_potential_list, p = bulk_cap_potential_probs))
        recommendedParameters["basket_value_cap"] = int(np.random.choice(basket_value_cap_potential_list, p = basket_value_cap_potential_probs))
    elif segment_label == 'AB':
        recommendedParameters["pills_cap"] = int(np.random.choice(pills_cap_potential_list, p = pills_cap_potential_probs))
        recommendedParameters["bulk_cap"] = int(np.random.choice(bulk_cap_potential_list, p = bulk_cap_potential_probs))
        recommendedParameters["basket_value_cap"] = int(np.random.choice(basket_value_cap_potential_list, p = basket_value_cap_potential_probs))
    elif segment_label == 'AC':
        recommendedParameters["pills_cap"] = int(np.random.choice(pills_cap_potential_list, p = pills_cap_potential_probs))
        recommendedParameters["bulk_cap"] = int(np.random.choice(bulk_cap_potential_list, p = bulk_cap_potential_probs))
        recommendedParameters["basket_value_cap"] = int(np.random.choice(basket_value_cap_potential_list, p = basket_value_cap_potential_probs))
    elif segment_label == 'BA':
        recommendedParameters["pills_cap"] = int(np.random.choice(pills_cap_potential_list, p = pills_cap_potential_probs))
        recommendedParameters["bulk_cap"] = int(np.random.choice(bulk_cap_potential_list, p = bulk_cap_potential_probs))
        recommendedParameters["basket_value_cap"] = int(np.random.choice(basket_value_cap_potential_list, p = basket_value_cap_potential_probs))
    elif segment_label == 'BB':
        recommendedParameters["pills_cap"] = int(np.random.choice(pills_cap_potential_list, p = pills_cap_potential_probs))
        recommendedParameters["bulk_cap"] = int(np.random.choice(bulk_cap_potential_list, p = bulk_cap_potential_probs))
        recommendedParameters["basket_value_cap"] = int(np.random.choice(basket_value_cap_potential_list, p = basket_value_cap_potential_probs))
    elif segment_label == 'BC':
        recommendedParameters["pills_cap"] = int(np.random.choice(pills_cap_potential_list, p = pills_cap_potential_probs))
        recommendedParameters["bulk_cap"] = int(np.random.choice(bulk_cap_potential_list, p = bulk_cap_potential_probs))
        recommendedParameters["basket_value_cap"] = int(np.random.choice(basket_value_cap_potential_list, p = basket_value_cap_potential_probs))
        
    return {"recommendedParameters": recommendedParameters}

def run(data):

    quiz_payload = json.loads(data)

    quiz_responses = quiz_payload['payload']['arguments']
    segment_label = segment_quiz_response(quiz_responses)
    recommendedParameters = get_parameters_for_segment(segment_label)
    
    return recommendedParameters

    