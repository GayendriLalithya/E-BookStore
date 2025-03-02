CREATE TABLE OrderItems (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    BookId INT NOT NULL,
    Quantity INT NOT NULL,
    FOREIGN KEY (OrderId) REFERENCES Orders(Id),
    FOREIGN KEY (BookId) REFERENCES Books(Id)
);
