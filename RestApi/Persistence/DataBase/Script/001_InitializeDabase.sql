CREATE TABLE IF NOT EXISTS users (
    id UUID PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    email VARCHAR(100) NOT NULL UNIQUE,
    password_hash VARCHAR(100) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE ExpenseCategory (
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    Name VARCHAR(255) NOT NULL
);


CREATE TABLE expenses (
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    Amount DECIMAL(10, 2) NOT NULL,
    Date TIMESTAMP NOT NULL,
    Description TEXT NOT NULL,
    ExpenseCategoryId UUID REFERENCES ExpenseCategory(Id) ON DELETE CASCADE
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS incomes (
    id UUID PRIMARY KEY,
    user_id UUID NOT NULL,
    amount DECIMAL(10, 2) NOT NULL,
    source VARCHAR(100),
    income_date DATE NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
);
