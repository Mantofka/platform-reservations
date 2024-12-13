# **Coliving Reservation System**

The **Coliving Reservation System** is designed to streamline dormitory (coliving) management tasks, saving time for the administration. The primary goals of the system include tracking room availability, managing payments for residents, and monitoring the maintenance history of rooms.

## **Functional Requirements**

The system supports four types of users: **Tenant**, **Coliving Owner**, and **Administrator**.

### **Tenant Capabilities**
- Submit a request to live in coliving.

### **Coliving Owner Capabilities**
- Submit requests to create new coliving spaces.
- Track room occupancy and availability.
- Create and edit coliving spaces and their information.
- Add or remove rooms.
- Update room details (e.g., amenities).

### **Administrator Capabilities**
- Edit tenant information

## **Technologies**

- **API**: The server-side API will be built using the `.NET` framework.
- **User Interface (UI)**: The front-end will be developed using the **Angular** JavaScript library.
- **Database**: Data storage and management will be handled by **PostgreSQL**.

**System architecture**

<img width="597" alt="Screenshot 2024-12-13 at 00 28 36" src="https://github.com/user-attachments/assets/831c6fda-f371-4d49-93e5-1b04fd847e27" />

**Client:**
- Accesses the system through a browser.
  
**Browser:**
- Firebase: Hosting the Angular frontend.
- DigitalOcean: Hosting the backend.
  
**DigitalOcean:**
Runs the .NET backend, which interacts with:

**PostgreSQL:**
The database.



**UI wireframes:**

Dashboard:
<img width="582" alt="Screenshot 2024-12-12 at 23 29 19" src="https://github.com/user-attachments/assets/eadeefdc-719d-42a9-9369-8f30ebf7306f" />

<img width="1437" alt="Screenshot 2024-12-12 at 23 49 21" src="https://github.com/user-attachments/assets/bcb87cb7-a0d4-4cf4-9d73-009abbfecae6" />



Coliving create/update screen
<img width="352" alt="Screenshot 2024-12-12 at 23 31 24" src="https://github.com/user-attachments/assets/b2f078c8-d3dd-413a-8cec-90f8ed0d3f5b" />

<img width="1420" alt="Screenshot 2024-12-12 at 23 51 25" src="https://github.com/user-attachments/assets/85685f6d-f7d0-4948-8a3d-1b961c805470" />


Room create/update screen

<img width="446" alt="Screenshot 2024-12-12 at 23 47 12" src="https://github.com/user-attachments/assets/45ad8411-681e-49a5-869e-5ca535c515c4" />

<img width="1422" alt="Screenshot 2024-12-12 at 23 54 18" src="https://github.com/user-attachments/assets/a907b03f-906f-40c8-bb4e-95b59ba63dc5" />



Tenants list screen:

<img width="611" alt="Screenshot 2024-12-12 at 23 47 36" src="https://github.com/user-attachments/assets/406132d7-e779-4c51-89b1-a5e268fd10a1" />

<img width="1440" alt="Screenshot 2024-12-12 at 23 53 51" src="https://github.com/user-attachments/assets/743b02a1-cfce-42f2-81f1-8454ff330dab" />



**Conclusion**

The Coliving Reservation System is designed to enhance the management and user experience for modern coliving spaces. By integrating streamlined functionality for various user roles: Tenants, Coliving Owners, and Administrators, the platform ensures efficient operations and seamless interactions.

Built on robust technologies such as the .NET framework for backend API, Angular for a dynamic frontend interface, and PostgreSQL for secure data storage, the system is architected for scalability, reliability, and performance. Hosting services on Firebase and DigitalOcean further ensure high availability and optimized resource utilization.

With features ranging from room availability tracking to payment management, maintenance monitoring, and administrative control, the system simplifies coliving operations and enhances user satisfaction. By leveraging intuitive UI designs and clear workflows, it offers a user-friendly experience that addresses the needs of all stakeholders.

The Coliving Reservation System stands as a comprehensive solution to manage coliving spaces effectively, fostering a collaborative and hassle-free living environment.
