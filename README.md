# Support Ticket Classifier

This project uses machine learning to predict the priority of support tickets based on their descriptions.  
It integrates a client application, a web API, and a machine learning model to streamline support workflows.

A user will fill out a form to enter a support ticket, the model will then predict the priority of that ticket and apply it.  
These tickets will be visible in a JIRA-style layout for a support user to view.

---

## Project Goals
- Build an end-to-end support ticketing system with ML-assisted priority classification.
- Integrate machine learning into a .NET application stack.
- Focus on practical deployment and usability over perfect prediction accuracy.

---

## User Workflow
1. A customer submits a support ticket via the Blazor client  
2. The API forwards the ticket’s description to the model  
3. The model returns a prediction of the ticket’s priority  
4. The API then stores this ticket in the database  
5. Authorized users can view and manage tickets via the client interface  

---

## Model
The model will be trained on the following dataset:  
[Customer Support Ticket Dataset (Kaggle)](https://www.kaggle.com/datasets/suraj520/customer-support-ticket-dataset/data)

The model is created in the following kaggle notebook:
[Kaggle Notebook](https://www.kaggle.com/code/harrykeys/ticket-priority-classifier) (need to make public)

---

## Learning Objectives

### Client (Blazor)
- Implement Microsoft Identity for authentication
- Provide a smooth, responsive UI for creating and managing tickets

### API (ASP.NET Core)
- Build RESTful endpoints
- Handle ML model integration
- Secure endpoints and validate inputs

### Database (PostgreSQL)
- Design ticket schema
- Perform CRUD operations
- Use migration and versioning

### Machine Learning
- Train a priority classifier using ticket data
- Evaluate model performance
- Deploy the model for real-time inference using Flask

---

## Roadmap

### v1.0 – MVP (Core Features)
- [x] Create new tickets (form with validation)  
- [x] View tickets in columns by priority  
- [x] Update tickets (basic edit form)  
- [x] Query ML model for predicted priority on ticket creation  
- [x] Manually update priority and log correction  
- [x] Drag & drop tickets between columns (updates DB)  
---

### v1.1 – Polish (Usability + Feedback Loop)
- [x] Feed manual corrections back into a feedback dataset  
- [x] “Retrain model” button to reload ML with corrections  
- [x] Display ML prediction confidence (probabilities per class)  
- [x] Add UI polish:
  - [x] Confirmation toasts (success/error)  
  - [x] Loading indicators while saving/querying model  
  - [x] Empty states (“No tickets yet”)  
- [x] Error handling for API calls (friendly UI + retry)
- [x] Cancel button for retraining the model.
- [x] Implement Logging and Usage throttling middleware
- [ ] Be able to toggle between trained model and GPT call
---

### v1.2 – Production Ready (DevOps + Reliability)
- [ ] Deploy app + DB to Azure App Service + Azure SQL  
- [ ] Set up CI/CD pipeline (GitHub Actions or Azure DevOps)  
- [ ] Seed DB with realistic fake tickets for demo  
- [ ] Enable monitoring/logging with Azure Application Insights  
- [ ] Add audit trail (ticket history: who changed priority, when, old → new)  
- [ ] Add concurrency handling in EF Core (optimistic concurrency tokens)
- [ ] Integrate with .NET Aspire
- [ ] Implement Messaging Queues with either RabbitMQ or Azure Bus for background model retraining
---

### v1.3 – Enhancements
- [ ] Authentication & role-based access (Agents vs Managers)  
- [ ] Search & filter tickets (by customer, priority, status)  
- [ ] Export tickets to CSV/Excel  
- [ ] Add explainability to predictions (keywords or SHAP values)  
- [ ] Track model accuracy over time (how many predictions were corrected)  
- [ ] Short demo video + polished README with screenshots  
---

## Future Enhancements (Ideas Backlog)
- Notifications or email alerts for high-priority tickets  
- Role-based access control (admin, support, customer)  

---
