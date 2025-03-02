

# E-Book System

This project is a comprehensive online e-book store developed for E-Book PVT LTD, a prominent secondhand book seller in Sri Lanka. The system is part of the CS6004ES Application Development module and leverages the ASP.NET MVC architecture to provide a robust web application.

## Table of Contents
- [Features](#features)
- [Installation](#installation)
  - [System Requirements](#system-requirements)
  - [Install Visual Studio](#install-visual-studio)
  - [Install SQL Server](#install-sql-server)
  - [Install SQL Server Management Studio (SSMS)](#install-sql-server-management-studio-ssms)
  - [Clone the Repository](#clone-the-repository)
  - [Setting Up the ASP.NET Project](#setting-up-the-aspnet-project)
- [Usage](#usage)
  - [Customer Manual](#customer-manual)
  - [Admin Manual](#admin-manual)
- [Architecture and Design](#architecture-and-design)
  - [Use Case Diagram](#use-case-diagram)
  - [Class Diagram](#class-diagram)
  - [ER Diagram](#er-diagram)
- [Development Process](#development-process)
- [Reflective Letters](#reflective-letters)
- [Conclusion](#conclusion)

## Features
- **User Registration and Login:** Secure user authentication and account management.
- **Book Search and Details:** Efficient search functionality with detailed book descriptions.
- **Order Management:** Seamless cart and order processing with integrated payment gateway.
- **User Reviews:** Allows users to leave reviews and ratings on purchased books.
- **Admin Capabilities:** Complete administrative controls for managing books, customers, orders, and generating reports.

## Installation

### System Requirements
Ensure your system meets the following requirements:
- **Operating System:** Windows 10 or later
- **Processor:** 1.8 GHz or faster processor (Quad-core or better recommended)
- **Memory:** 2 GB of RAM (8 GB recommended, 2.5 GB minimum if running on a virtual machine)
- **Hard Disk Space:** Minimum of 800MB up to 210 GB of available space (typical installations require 20-50 GB of free space)
- **Screen Resolution:** 1280 x 720 or higher display resolution

### Install Visual Studio
1. **Download Visual Studio:**
   - Visit the [Visual Studio Download Page](https://visualstudio.microsoft.com/downloads/)
   - Choose the Visual Studio Community Edition (sufficient and free for students, open-source contributors, and individuals)
2. **Run the Installer:**
   - Download and run the Visual Studio installer
3. **Select Workloads:**
   - In the installer, go to the "Workloads" tab
   - Select "ASP.NET and web development" (includes all necessary tools and libraries for ASP.NET)
   - Optionally, select ".NET desktop development" for additional desktop app tools if needed
4. **Complete Installation:**
   - Follow the on-screen instructions to complete the installation

### Install SQL Server
1. **Download SQL Server:**
   - Visit the [SQL Server Download Page](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
   - Choose SQL Server Express Edition (free and sufficient for most development needs)
2. **Run the Installer:**
   - Choose the basic installation to set up SQL Server with default settings (for a more customized setup, select custom)
   - Follow the setup instructions, setting an instance name and configuring server settings as needed

### Install SQL Server Management Studio (SSMS)
1. **Download SSMS:**
   - Visit the [SSMS Download Page](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)
   - Download the latest version of SSMS
2. **Run the Installer:**
   - Follow the on-screen instructions to install SSMS

### Clone the Repository
1. **Install Git:**
   - If you don't have Git installed, download and install it from the [Git website](https://git-scm.com/)
2. **Clone the Repository:**
   - Open a command prompt or terminal
   - Run the following command to clone the repository:
     ```bash
     git clone https://github.com/Sahiru2007/EBook-Management-system-.git
     ```
   - Navigate to the cloned repository directory:
     ```bash
     cd EBook-Management-system-
     ```

### Setting Up the ASP.NET Project
1. **Open Visual Studio:**
   - Launch Visual Studio on your system
2. **Open the Project:**
   - Click on "Open a project or solution"
   - Navigate to the cloned repository directory and select the `.sln` file
3. **Restore NuGet Packages:**
   - Open the NuGet Package Manager Console (Tools > NuGet Package Manager > Package Manager Console)
   - Run the command `Update-Package -reinstall` to restore any missing packages
4. **Update Database Connection String:**
   - Open `appsettings.json` or `Web.config` (depending on your project setup)
   - Update the connection string to match your SQL Server instance details
5. **Run the Database Migrations:**
   - Open the Package Manager Console
   - Run the command `Update-Database` to apply migrations and create the database schema

## Usage

### Customer Manual
1. **Sign Up:**
   - Go to the Home page and click on the "Sign Up" button
   - Fill in the registration form with your details and click "Register"
   - Confirm your registration via the confirmation email
2. **Login:**
   - Go to the Home page and click on the "Login" button
   - Enter your email and password, then click "Login"
3. **Search and Find Books:**
   - Navigate to the Book Store page
   - Use the search bar to find books by title, author, or keyword
   - Apply filters to narrow down search results
4. **View Book Details:**
   - Hover over a book and click the “Eye” icon
   - View detailed information about the book, including description, author, and price
5. **Buy a Book:**
   - Select a book and click "Buy Now"
   - Enter your shipping details
   - Complete the purchase via the Stripe payment gateway
6. **Add to Cart:**
   - Hover over a book and click the "Shopping Bag" icon to add it to your cart
7. **User Reviews:**
   - Navigate to the bottom of a book's detail page
   - Write your review in the text box and submit it

### Admin Manual
1. **Admin Login:**
   - Go to the Admin Login page
   - Enter your admin credentials (e.g., Username: “admin@gmail.com”, Password: “123”) and click "Submit"
2. **View Admin Dashboard:**
   - Login and navigate to the dashboard via the sidebar
3. **Add Books:**
   - Select "Books" in the sidebar
   - Enter the book details and submit the form to add a new book
4. **View and Manage Books:**
   - Navigate to the "Manage Book" tab
   - View book details, edit book information, or delete a book
5. **Order Management:**
   - Go to the "Order" tab
   - View and update the status of orders (e.g., Placed, Processed, Shipped, Delivered)
6. **View and Manage Customers:**
   - Navigate to the "Manage Customers" section
   - View the list of customers, edit their details, or deactivate accounts
7. **View and Manage Reviews:**
   - Navigate to the "Manage Reviews" section
   - View and delete reviews as necessary
8. **Generate Reports:**
   - Generate sales and customer activity reports, exportable to PDF or CSV

## Architecture and Design
<img width="828" alt="Screenshot 2024-06-24 at 20 40 05" src="https://github.com/Sahiru2007/EBook-Management-system-/assets/75121314/5771d353-f912-42c5-bc8b-7df15337f7fb">

## Use Case Diagram
<img width="728" alt="Screenshot 2024-06-24 at 20 40 38" src="https://github.com/Sahiru2007/EBook-Management-system-/assets/75121314/7721de93-4746-4349-832c-1ebe058f28f9">

### Class Diagram
<img width="879" alt="Screenshot 2024-06-24 at 20 41 04" src="https://github.com/Sahiru2007/EBook-Management-system-/assets/75121314/39f0c648-f5a3-48e4-a3e0-69e5d7cb7be6">

### ER Diagram
<img width="1411" alt="Screenshot 2024-06-24 at 20 41 36" src="https://github.com/Sahiru2007/EBook-Management-system-/assets/75121314/62bcdcc6-d553-4c0f-ad00-d668427573de">

## Development Process
The development process involves several steps, including setting up the development environment, designing the system architecture, implementing the functionality, and testing the application. The following sections provide a detailed explanation of each step.

## Reflective Letters
Reflective letters from team members are included, highlighting the challenges faced, solutions implemented, and the learning experience throughout the project.

## Conclusion
The E-Book System project successfully modernizes E-Book PVT LTD's business model, enhancing operational efficiency and customer engagement. The project showcases the application of theoretical knowledge in a practical setting, providing valuable insights into web application development using ASP.NET MVC.

---

This detailed `README.md` file provides all the necessary information about your E-Book System project, including features, installation instructions, usage guidelines, and more.
