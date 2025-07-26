-- Manual SQL script to add AccountNumber column to Payees table
-- This implements the same changes as the AddAccountNumberToPayees migration

USE [MyBank(3)];
GO

-- Step 1: Add the new AccountNumber column (user's account who added this payee)
ALTER TABLE [Payees] 
ADD [AccountNumber] bigint NOT NULL DEFAULT 0;
GO

-- Step 2: Create index for the new AccountNumber column
CREATE INDEX [IX_Payees_AccountNumber] ON [Payees] ([AccountNumber]);
GO

-- Step 3: Add foreign key constraint for AccountNumber (user's account)
ALTER TABLE [Payees]
ADD CONSTRAINT [FK_Payees_Accounts_AccountNumber] 
FOREIGN KEY ([AccountNumber]) 
REFERENCES [Accounts] ([AccountNumber])
ON DELETE NO ACTION;
GO

-- Step 4: Update the migration history to mark this migration as applied
INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES ('20250726064000_AddAccountNumberToPayees', '8.0.0');
GO

PRINT 'Migration completed successfully. AccountNumber column added to Payees table.';