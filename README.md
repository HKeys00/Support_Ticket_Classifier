# Support Ticket Classifier

This project uses machine learning to predict the priority of support tickets based on their descriptions. 
It integrates a client application, a web API, and a machine learning model to streamline support workflows.

A user will fill out a form to enter a support ticket, the model will then predict the priority of that ticket and apply it.
These tickets will be visible in a JIRA style layout for a support user to view.

## Project Goals
- Build an end-to-end support ticketing system with ML-assisted priority classification.
- Integrate machine learning into a .NET application stack.
- Focus on practical deployment and usability over perfect predicition accuracy.

## User Workflow
	- A customer submits a support ticket via the Blazor client
	- The API forwards the ticket's description to the model
	- The model returns a prediction of the ticket's priority
	- The API then stores this ticket in the database.
	- Authorized users can view and manage tickets via the client interface.

## Dataset
The model will be trained on the following dataset: https://www.kaggle.com/datasets/suraj520/customer-support-ticket-dataset/data

## Learning Objectives
Client (Blazor)
- Implement Microsoft Identity for authentication

API (ASP.NET Core)
- Build RESTful endpoints
- Handle ML model integration
- Secure endpoints and validate inputs

Database (PostgreSQL)
- Design ticket schema
- Perform CRUD operations
- Use migration and versioning

Machine Learning
- Train a priority classifier using ticket data.
- Evaluate model performance (F1 Score)
- Deploy the model for real-time inference

## Future enhancements
- Manual override of predicted priorities
- Search for tickets
- JIRA-style kanban board with drag-and-drop
- Ticket status tracking (open, in progress, resolved)
- Notifications or email alerts for high-priority tickets
- Role-based access control(admin, support, customer)
