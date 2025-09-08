from flask import Flask, request, jsonify
import numpy as np

def to_dense_transform(x):
    return x.toarray() if hasattr(x, "toarray") else x

import joblib
import pandas as pd

app = Flask(__name__)
model = joblib.load('ticket_classifier_model.pkl')

@app.route('/', methods=['GET'])
def predict():
    new_ticket = pd.DataFrame([{
        "Date of Purchase": "2021-07-15",
        "Ticket Type": "Refund request",
        "Ticket Subject": "Login issue",
        "Ticket Description": "I can't log in to my account after purchase",
        "Ticket Channel": "Email"
    }])

    prediction = model.predict(new_ticket)
    
    return jsonify({
        "prediction": prediction[0]
    })
    
if __name__ == '__main__':
    app.run(debug=True, port=3000)

