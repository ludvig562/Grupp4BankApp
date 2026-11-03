A console-based banking application built as a group project by [**Svea**](https://github.com/Sfejaaa369), [**Mathias**](https://github.com/TofflanDev), [**Ludvig**](https://github.com/ludvig562), and [**Yvegeniy**](https://github.com/swimman88-spec).

## Overview

The application simulates a small bank with administrator and customer accounts. Users can log in, manage bank accounts, perform transactions, view transaction history and statistics, manage loans, and work with exchange rates.

The program includes hardcoded sample data with one administrator and two customers. It also uses a timer and background thread to process scheduled transactions after a 15-minute waiting period.

## Features

- User authentication with password hashing and login-attempt tracking
- Separate administrator and customer menus
- Bank account creation and management
- Deposits, withdrawals, and fund transfers
- Scheduled transactions with delayed execution
- Transaction logging and statistics
- Exchange-rate loading from an external site
- Loan creation, eligibility checks, and interest calculations
- Customer information updates
- Notifications and application configuration

## Project Structure

| Classes | Responsibility |
| --- | --- |
| `Program` | Starts the application, initializes sample users, and starts the transaction timer. |
| `User` | Stores usernames, passwords, and personal user information. Base class for customers and administrators. |
| `Customer` | Manages customer information, bank accounts, deposits, withdrawals, account creation, and account overviews. |
| `Admin` | Provides administrator operations such as creating customers, saving daily exchange rates, and updating loan interest. |
| `BankAccount` | Stores account type, account number, balance, and currency. Handles deposits, withdrawals, and savings interest. |
| `Transaction` | Manages transaction timing, transfers, deposits, withdrawals, and transaction-log entries. |
| `PendingTransaction` | Stores scheduled transactions until they are ready to execute. |
| `TransactionLogg` | Stores transaction log data. |
| `TransactionStatistics` | Provides transaction statistics. |
| `AuthenticationService` | Authenticates users, tracks login attempts, and handles logout. |
| `PasswordHasher` | Generates salts and hashes or verifies passwords using PBKDF2 with SHA-256. |
| `ExchangeRate` | Loads and displays exchange rates from an external source. |
| `Loan` | Handles loan menus, loan creation, eligibility checks, and interest calculations. |
| `LoanInterestRules` | Stores credit-score and loan-length rules and updates loan properties. |
| `NotificationService` | Contains notification logic. |
| `Enums` | Defines enumerations shared by other components. |
| `AdminMenu` | Displays the administrator menu and routes selections to administrator methods. |
| `CustomerMenu` | Displays the customer menu and routes selections to customer methods. |
| `LogInMenu` | Displays the bank logo and login interface. |
| `MenuManager` | Runs the login flow and selects the appropriate administrator or customer menu. |
| `AppConfig` | Stores application configuration properties. |

## Console UI

The application uses:

- **Spectre.Console** for menus, styling, and console animations
- **Figgle** for the bank logo displayed in the login menu

## Running the Application

1. Open `Grupp4BankApp.slnx` in a compatible .NET development environment.
2. Build the solution.
3. Run the application project.
4. Log in using one of the sample users configured in `Program`.

## Notes

Transactions are scheduled for 15 minutes after creation before they are processed. Passwords are protected using salted PBKDF2 hashing with SHA-256.

