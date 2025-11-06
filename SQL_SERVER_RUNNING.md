# SQL Server Status - ✅ RUNNING

## Current Status

**SQL Server Service**: ✅ **RUNNING**
- Service Name: `MSSQLSERVER`
- Status: **Running**
- Display Name: SQL Server (MSSQLSERVER)

---

## Connection Details

### EntityFrameworkCRUD Connection

**Connection String**:
```
Server=.;Database=CustomerDb;Trusted_Connection=True;TrustServerCertificate=True;
```

**Connection Details**:
- **Server**: `.` (local default instance)
- **Database**: `CustomerDb` (will be created automatically)
- **Authentication**: Windows Authentication
- **Status**: ✅ Ready to use

---

## Testing the Connection

### Run EntityFrameworkCRUD

```bash
cd EntityFrameworkCRUD
dotnet run
```

**What happens**:
1. ✅ Connects to SQL Server (already running)
2. ✅ Creates `CustomerDb` database automatically (if it doesn't exist)
3. ✅ Seeds 20 sample customers using Bogus
4. ✅ Starts the application

---

## Verify Database Creation

After running EntityFrameworkCRUD, you can verify the database was created:

### Using SQL Server Management Studio (SSMS)

1. Open SQL Server Management Studio
2. Connect to: `localhost` or `.`
3. Expand "Databases"
4. Look for `CustomerDb` database
5. Expand `CustomerDb` → Tables → `Customers`
6. Right-click `Customers` → "Select Top 1000 Rows" to view data

### Using Command Line

```powershell
# List all databases
sqlcmd -S . -Q "SELECT name FROM sys.databases"

# Check if CustomerDb exists
sqlcmd -S . -Q "SELECT name FROM sys.databases WHERE name = 'CustomerDb'"

# Count customers in database
sqlcmd -S . -d CustomerDb -Q "SELECT COUNT(*) FROM Customers"
```

---

## Service Management

### Check SQL Server Status

```powershell
Get-Service -Name "MSSQLSERVER" | Select-Object Status, Name, DisplayName
```

### Start SQL Server (if stopped)

```powershell
Start-Service -Name "MSSQLSERVER"
```

### Stop SQL Server

```powershell
Stop-Service -Name "MSSQLSERVER"
```

### Restart SQL Server

```powershell
Restart-Service -Name "MSSQLSERVER"
```

---

## Summary

✅ **SQL Server is installed and running**  
✅ **Service MSSQLSERVER is active**  
✅ **Ready to use with EntityFrameworkCRUD**  
✅ **Database will be created automatically on first run**

**No action needed!** SQL Server is running and ready to use.

---

## Next Steps

1. ✅ SQL Server is running - **DONE**
2. ✅ Run EntityFrameworkCRUD to create database:
   ```bash
   cd EntityFrameworkCRUD
   dotnet run
   ```
3. ✅ Database will be created automatically
4. ✅ Test the application

---

## Troubleshooting

### If Connection Fails

1. **Verify SQL Server is running**:
   ```powershell
   Get-Service -Name "MSSQLSERVER"
   ```

2. **Check SQL Server Error Log**:
   - Location: `C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\Log\`
   - Or use SQL Server Management Studio → Management → SQL Server Logs

3. **Test Connection**:
   ```powershell
   sqlcmd -S . -Q "SELECT @@VERSION"
   ```

4. **Check Firewall**:
   - SQL Server uses port 1433
   - Ensure Windows Firewall allows SQL Server connections

---

## Resources

- **SQL Server Management Studio**: https://aka.ms/ssmsfullsetup
- **SQL Server Documentation**: https://docs.microsoft.com/en-us/sql/sql-server/
- **Connection Strings**: https://www.connectionstrings.com/sql-server/

