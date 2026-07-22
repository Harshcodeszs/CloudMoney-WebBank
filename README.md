# CloudMoney — Banking Web Application

CloudMoney is a secure, web-based banking application designed to handle user accounts, transaction processing, and automated financial reporting. The system allows users to register, manage their account balances, perform deposits and withdrawals, transfer funds securely to other users, and track detailed transaction history.

---

## 🚀 Key Features & Modules

### 🔐 Authentication & Account Management
* **User Registration:** Allows new users to create an account and assigns them a unique Account Number.
* **User Login / Logout:** Secure authentication session management.
* **Change Password:** Enables users to update their credentials securely.
* **Dashboard:** Displays real-time summary details for the logged-in user:
  * Account Number, Full Name, and Date Registered
  * Total Current Balance and Total Sent Amount
  * Notifications for recently received transfers
  * Embedded **General Inquiry Chatbot**

---

### 💳 Transaction Modules & Business Logic

#### 1. Deposit
* **Minimum per transaction:** ₱100.00
* **Maximum per transaction:** ₱2,000.00
* **Increment Rule:** Amount must be divisible by 100.00
* **Balance Cap:** Total current balance for a user cannot exceed **₱10,000.00**

#### 2. Withdrawal
* Displays the current signed-in user's balance prior to execution.
* **Minimum per transaction:** ₱100.00
* **Maximum per transaction:** ₱2,000.00
* **Increment Rule:** Amount must be divisible by 100.00
* **Validation:** Prevents transactions if funds are insufficient.

#### 3. Send CloudMoney
* Recipient verification via Account Number lookup (displays recipient's Account No. and Name upon verification).
* **Minimum per transaction:** ₱100.00
* **Maximum per transaction:** ₱2,000.00
* **Increment Rule:** Amount must be divisible by 100.00
* **Security:** Requires password confirmation from the logged-in user before processing.
* **Validation:** Prevents transaction if sending funds exceed available balance.

---

## 📊 Reports & Transaction History

All reporting modules enforce strict date range validations:
* **Date Validation:** `From Date` and `To Date` cannot be future dates, and `From Date` must be strictly earlier than `To Date`.

1. **Statement of Account:** Complete audit trail showing all transaction history (Deposits, Withdrawals, Sent, and Received transfers) for the logged-in user.
2. **My Deposits or Withdrawals:** Filterable transaction report supporting filter types: `All`, `Deposit`, or `Withdrawal`.
3. **My Sent or Received Transactions:** Filterable transfer report supporting filter types: `All`, `Sent`, or `Received`.

---

## 🛠️ Tech Stack

* **Frontend / Web Forms:** ASP.NET Web Forms (HTML5, CSS3, Bootstrap)
* **Backend:** C# (.NET Framework)
* **Database:** SQL Server / MySQL (ACID-compliant transactions for financial safety)

---

## 🏁 Getting Started

### Prerequisites
* [Visual Studio](https://visualstudio.microsoft.com/) (with ASP.NET & Web Development workload)
* [SQL Server](https://www.microsoft.com/en-us/sql-server/) or equivalent database engine
* .NET Framework 4.8+

### Setup Instructions

1. **Clone the repository:**
   ```bash
   git clone [https://github.com/your-username/cloud-money-webapp.git](https://github.com/your-username/cloud-money-webapp.git)
   cd cloud-money-webapp
