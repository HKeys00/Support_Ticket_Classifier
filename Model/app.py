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
    data = request.get_json(force=True)
    print(data)
    new_ticket = pd.DataFrame([data])
    new_ticket = to_dense_transform(new_ticket)
    prediction = model.predict(new_ticket)
    
    return jsonify({
        "prediction": prediction[0]
    })
    
if __name__ == '__main__':
    app.run(debug=True, port=3000)

