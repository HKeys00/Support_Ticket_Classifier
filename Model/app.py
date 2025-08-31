from flask import Flask, request
import joblib
import pandas as pd

app = Flask(__name__)
model = joblib.load('ticket_classifier_model.pkl')

@app.route('/', methods=['POST'])
def predict():
    data = request.json
    print(data)
    
if __name__ == '__main__':
    app.run(debug=True, port=3000)
