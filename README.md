Svea: https://github.com/Sfejaaa369
Mathias: https://github.com/TofflanDev 
Ludvig: https://github.com/ludvig562
Yvegeniy: https://github.com/swimman88-spec

Program: contains hardcoded users, one admin and two customers. Starts and initiates the program and starts a timer for a new thread.

User: : contains dictionary with all usernames and passwords. Holds all the personal information of a user like name, password, e-mail, etc. It is then inherited to create the Customer and Admin classes.

Customer: holds information like which bank accounts are associated with that customer. Customer also holds the methods that are called from the customer menu: methods that allow the customer to open a new bank account, deposit and withdraw money from their accounts, change their personal information and show an overview of all their bank accounts. 

Admin: holds (most of) the methods called from the adminmenu: methods to save new daily exchange rates, to create a new customer and update loan interest.

Pendingtransaction: holds method for saving scheduled transaction for 15 minutes after creation before they get executed. 

Transaction: holds variables for threading. Hold methods related to transactions: method to start a timer, process pending transactions, transfer funds after the waiting time has passed, add transactions (both transfers and deposits/withdrawals) to transaction log and a method to show the transaction log.

TransactionLogg: contains transactions log data.

TransactionStatistics: holds methods for showing transactions statistics.

AuthenticationService: holds methods to authenticate a user's login. Checks the username and password,as well as amount of times tried to log in. Also holds method for logging out.

BankAccount: holds informationrelated to the bank accounts, such as account type, bank account number, bank account balance and currency. Also holds methods to deposit and withdraw money into the account and to calculate the savings account interest.

Enums: contains multiple enums that are used in other classes.

ExchangeRate: contains variables for the exchange rates and methods to load rates from an external site and show in our bankapp.

Loan: holds variables related to loan, such as amount and interest rate. Also holds methods to show the loan menu, create a loan and check if customer is allowed to loan money and calculate the loan interest.

LoanInteresRules: contains variables related to the user credit score rating and the loan length. Also contains methods to update the properties of the loan.

NotificationService: contains notification logic. 

PasswordHasher: contains method to create a random generated salt. Hashes password with PBKDF2 using SHA256. And contains a method to verify using the same algorithm.

AdminMenu: contains a mehtod that shows the admin menu. Each case calls on a method from the admin class (except for maybe update exchange rate?).

CustomerMenu: contains animation log in event and method that shows the Customer Menu with the library Spector Console. Each case in the menu calls on methods from the Customer class.

LogInMenu: shows the bank logo from the library Figgle and the login UI is showsn with the library Spector Console. This class shows the log in menu there the users can log in.

MenuManager: It runs the login menu, and decides which menu they get shown (admin or customer).

AppConfig: saves configuration property.
