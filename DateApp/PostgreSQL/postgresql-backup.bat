@set PGPASSFILE=PostgreSQL\13243110-5833-400c-b1ba-10f63c2e741f.conf
@"D:\POSTGRES\bin\pg_dump.exe" -Fc -h localhost -p 5433 -d dateapp -U postgres  > "I:\BD_Coursework_6\DateApp\DateApp\Backup\temp.sql"
