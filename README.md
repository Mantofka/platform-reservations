# **Coliving Reservation System**

The **Coliving Reservation System** is designed to streamline dormitory (coliving) management tasks, saving time for the administration. The primary goals of the system include tracking room availability, managing payments for residents, and monitoring the maintenance history of rooms.

## **Functional Requirements**

The system supports four types of users: **Guest**, **Resident**, **Dormitory Administrator**, and **System Administrator**.

### **Guest Capabilities**
- View available coliving spaces.
- Log in.
- Register.
- Submit a request to live in coliving.
  - *(Note: At the final stage of the form, users will be asked to log in or register.)*

### **Resident Capabilities**
- Track payment history.
- Make payments.
- Report maintenance issues.
- Monitor the status of maintenance requests.
- Extend their stay.
- Submit a request to live in coliving.

### **Dormitory Administrator Capabilities**
- Submit requests to create new coliving spaces.
- Track room occupancy and availability.
- Create and edit coliving spaces and their information.
- Add or remove rooms.
- Update room details (e.g., amenities).
- Change the status of maintenance history (e.g., resolve issues).
- Send payment requests to residents.

### **System Administrator Capabilities**
- Approve or reject requests for creating new coliving spaces.
- Manage system-wide settings and configurations.

## **Technologies**

- **API**: The server-side API will be built using the `.NET` framework.
- **User Interface (UI)**: The front-end will be developed using the **React** JavaScript library.
- **Database**: Data storage and management will be handled by **PostgreSQL**.
