# Support Ticket Classifier

This project uses machine learning to predict the priority of support tickets based on their descriptions. 
It integrates a client application, a web API, and a machine learning model to streamline support workflows.

A user will fill out a form to enter a support ticket, the model will then predict the priority of that ticket and apply it.
These tickets will be visible in a JIRA style layout for a support user to view.

The goal of this project is not to get the ML model 100% correct, it's to get an fairly accurate model that I can then deploy
and query for ticket priorities.

User Workflow
	- A customer submits a support ticket via the Blazor client
	- The API forwards the ticket's description to the model
	- The model returns a prediction of the ticket's priority
	- The API then stores this ticket in the database.
	- Authorized users can view and manage tickets via the client interface.


SmartSupportAPI
	The SmartSupportAPI will act as the middle man between the clients, the server and the machine learning model.
	This will be an ASP.NET Core Web API application
		Things I need to learn:
			- Networking protocols
			- ASP.NET Core

Client
	The Client will allow customers to enter a new support ticket (and potentially view their existing tickets), it will also allow authorised support workers to view and edit existing support tickets
	This will be a .NET 9 Blazor Application
		Things I need to learn:
			- Microsoft Authentication

Database
	The database will store the support tickets for the application
	This will be a PostgreSQL database
		Things I need to learn:
			- PostgreSQL

ML Model
	This model is what will be used to predict the priority of a ticket given it's description
	It will be trained on the following dataset: https://www.kaggle.com/datasets/suraj520/customer-support-ticket-dataset/data
		Things I need to learn:
			- Training and Deploying a working model