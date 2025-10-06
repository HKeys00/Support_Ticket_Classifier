from flask import Flask, request, jsonify
import numpy as np
from retrain import retrain_model

def to_dense_transform(x):
    return x.toarray() if hasattr(x, "toarray") else x

import joblib
import pandas as pd

app = Flask(__name__)
model = joblib.load('ticket_classifier_model.pkl')

@app.route('/predict', methods=['POST'])
def predict():
    data = request.get_json(force=True)
    new_ticket = pd.DataFrame([{
        "Date of Purchase": data["dateOfPurchase"],
        "Ticket Type": data["type"],
        "Ticket Subject": data["subject"],
        "Ticket Description": data["description"],
        "Ticket Channel": data["channel"]
    }])

    predicted_class = int(model.predict(new_ticket)[0])
    class_probs = model.predict_proba(new_ticket)[0]
    class_confidence = [float(prob) for cls, prob in zip(model.classes_, class_probs)]
    return jsonify({
    "Prediction": predicted_class,
    "Confidence": class_confidence
    })


@app.route('/retrain', methods=['POST'])
def retrain():
    data = request.get_json(force=True)
    


    retrain_model()

    model = joblib.load('ticket_classifier_model.pkl')
if __name__ == '__main__':
    app.run(debug=True, port=3000)

