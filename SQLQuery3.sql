CREATE TABLE Reviews (
    Id INT PRIMARY KEY IDENTITY,
    BookId INT,
    UserId NVARCHAR(450),
    ReviewText NVARCHAR(MAX),
    Timestamp DATETIME,
    FOREIGN KEY (BookId) REFERENCES Books(Id),
    FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id)
);