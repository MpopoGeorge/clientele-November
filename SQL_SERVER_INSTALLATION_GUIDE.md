# SQL Server Installation Guide

## For EntityFrameworkCRUD Project

This guide will help you install SQL Server on Windows for the EntityFrameworkCRUD project.

---

## Option 1: SQL Server Express (Recommended for Development)

**SQL Server Express** is a free, lightweight version of SQL Server perfect for development.

### Download SQL Server Express

1. **Visit Microsoft SQL Server Downloads**:
   - Go to: https://www.microsoft.com/en-us/sql-server/sql-server-downloads
   - Click "Download now" under **SQL Server Express**

2. **Or Direct Download Link**:
   - SQL Server 2022 Express: https://www.microsoft.com/en-us/sql-server/sql-server-downloads
   - Choose "Express" edition (free)

### Installation Steps

1. **Run the Installer**:
   - Run the downloaded `.exe` file
   - Choose "Basic" installation for quick setup
   - Or choose "Custom" for more control

2. **Basic Installation** (Recommended):
   - Accept the license terms
   - Choose installation location (default is fine)
   - Click "Install"
   - Wait for installation to complete
   - Note the instance name (usually `SQLEXPRESS`)

3. **Custom Installation** (Advanced):
   - Select "New SQL Server stand-alone installation"
   - Accept license terms
   - Choose features:
     - ✅ Database Engine Services (required)
     - ✅ SQL Server Replication (optional)
     - ✅ Full-Text and Semantic Extractions for Search (optional)
   - Choose instance configuration:
     - Default instance or named instance (e.g., `SQLEXPRESS`)
   - Server Configuration:
     - Choose authentication mode:
       - **Windows Authentication** (recommended for development)
       - Or **Mixed Mode** (SQL Server + Windows Authentication)
   - Complete the installation

### Verify Installation

After installation, verify SQL Server is running:

```powershell
# Check SQL Server service
Get-Service -Name '*SQL*' | Where-Object {$_.Status -eq 'Running'}

# Test connection
sqlcmd -S . -Q "SELECT @@VERSION"
```

---

## Option 2: SQL Server LocalDB (Lightweight)

**SQL Server LocalDB** is a lightweight version that starts automatically when needed.

### Installation

1. **Download SQL Server Express with LocalDB**:
   - Visit: https://www.microsoft.com/en-us/sql-server/sql-server-downloads
   - Download "Express" edition
   - During installation, select "LocalDB" feature

2. **Or Install via Visual Studio**:
   - SQL Server LocalDB is included with Visual Studio
   - If you have Visual Studio installed, LocalDB may already be available

### Verify LocalDB Installation

```powershell
# Check LocalDB
sqllocaldb info

# Start LocalDB instance
sqllocaldb start MSSQLLocalDB

# Create and connect to LocalDB
sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "SELECT @@VERSION"
```

### Update Connection String for LocalDB

If using LocalDB, update `EntityFrameworkCRUD/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=CustomerDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

---

## Option 3: SQL Server Developer Edition (Free)

**SQL Server Developer Edition** is free and includes all features of Enterprise Edition (for development only).

### Download

1. Visit: https://www.microsoft.com/en-us/sql-server/sql-server-downloads
2. Choose "Developer" edition
3. Download and install

### Installation

- Similar to SQL Server Express
- Full-featured for development
- Free license for development use

---

## Quick Installation Script

### Using Chocolatey (Package Manager)

If you have Chocolatey installed:

```powershell
# Install SQL Server Express
choco install sql-server-express -y

# Or install LocalDB
choco install sqlserver-express-localdb -y
```

### Using Winget (Windows Package Manager)

```powershell
# Install SQL Server Express
winget install Microsoft.SQLServer.Express

# Or install LocalDB
winget install Microsoft.SQLServer.LocalDB
```

---

## Post-Installation Configuration

### 1. Enable SQL Server Authentication (if needed)

If you need SQL Server Authentication instead of Windows Authentication:

1. Open **SQL Server Management Studio (SSMS)**
2. Connect to your SQL Server instance
3. Right-click server → Properties → Security
4. Select "SQL Server and Windows Authentication mode"
5. Restart SQL Server service

### 2. Update Connection String

If you installed a named instance (e.g., `SQLEXPRESS`), update the connection string:

**File**: `EntityFrameworkCRUD/appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=CustomerDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Or for LocalDB:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=CustomerDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 3. Install SQL Server Management Studio (SSMS)

**SSMS** is a GUI tool for managing SQL Server databases.

1. Download: https://aka.ms/ssmsfullsetup
2. Install SSMS
3. Connect to your SQL Server instance
4. View and manage databases

---

## Verify Installation

### Test SQL Server Connection

```powershell
# Test connection to default instance
sqlcmd -S . -Q "SELECT @@VERSION"

# Test connection to named instance (e.g., SQLEXPRESS)
sqlcmd -S .\SQLEXPRESS -Q "SELECT @@VERSION"

# Test LocalDB
sqlcmd -S "(localdb)\MSSQLLocalDB" -Q "SELECT @@VERSION"
```

### Check SQL Server Service Status

```powershell
# Check all SQL Server services
Get-Service -Name '*SQL*' | Format-Table -AutoSize

# Start SQL Server service (if stopped)
Start-Service -Name "MSSQLSERVER"
# Or for named instance
Start-Service -Name "MSSQL$SQLEXPRESS"
```

### Test EntityFrameworkCRUD Project

```bash
cd EntityFrameworkCRUD
dotnet run
```

The application will:
1. Connect to SQL Server
2. Create the `CustomerDb` database automatically
3. Seed 20 sample customers
4. Start the application

---

## Troubleshooting

### Issue: "Cannot connect to SQL Server"

**Solution**:
1. Check if SQL Server service is running:
   ```powershell
   Get-Service -Name '*SQL*'
   ```

2. Start SQL Server service:
   ```powershell
   Start-Service -Name "MSSQLSERVER"
   ```

3. Check firewall settings (SQL Server uses port 1433)

### Issue: "Login failed for user"

**Solution**:
1. Ensure Windows Authentication is enabled
2. Or use SQL Server Authentication with username/password
3. Update connection string accordingly

### Issue: "Database does not exist"

**Solution**:
- This is normal! The application will create the database automatically
- Run `dotnet run` and the database will be created

### Issue: "Named instance not found"

**Solution**:
1. Find your SQL Server instance name:
   ```powershell
   Get-Service -Name '*SQL*' | Select-Object Name
   ```

2. Update connection string with correct instance name:
   ```json
   "DefaultConnection": "Server=.\\INSTANCENAME;Database=CustomerDb;..."
   ```

---

## Recommended Setup for Development

**For Quick Development**:
- ✅ SQL Server Express with LocalDB
- ✅ Use Windows Authentication
- ✅ Default instance or LocalDB

**Connection String** (Default instance):
```json
"DefaultConnection": "Server=.;Database=CustomerDb;Trusted_Connection=True;TrustServerCertificate=True;"
```

**Connection String** (LocalDB):
```json
"DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=CustomerDb;Trusted_Connection=True;TrustServerCertificate=True;"
```

---

## Next Steps

1. ✅ Install SQL Server (Express, LocalDB, or Developer)
2. ✅ Verify installation
3. ✅ Update connection string if needed (for named instances)
4. ✅ Run EntityFrameworkCRUD project:
   ```bash
   cd EntityFrameworkCRUD
   dotnet run
   ```
5. ✅ Database will be created automatically
6. ✅ Test the application

---

## Resources

- **SQL Server Downloads**: https://www.microsoft.com/en-us/sql-server/sql-server-downloads
- **SQL Server Management Studio**: https://aka.ms/ssmsfullsetup
- **SQL Server Documentation**: https://docs.microsoft.com/en-us/sql/sql-server/
- **Connection Strings**: https://www.connectionstrings.com/sql-server/

---

## Summary

✅ **SQL Server Express** - Free, good for development  
✅ **SQL Server LocalDB** - Lightweight, auto-starts  
✅ **SQL Server Developer** - Full features, free for dev  

**Recommended**: SQL Server Express or LocalDB for development

After installation, the EntityFrameworkCRUD project will automatically create the database on first run!

