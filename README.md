# .NET-API-With-DDD

Foi utilizado o user-secrets para guardar as informações de conexão com DB e Configuraçao de JWT

Executes os seguintes comandos no 1 - Manager.API:
1. dotnet user-secrets init --project .\Manager.API.csproj

#SqlServer
2. dotnet user-secrets set "ConnectionStrings:USER_MANAGER" "Server=localhost;Database=USERMANAGERAPI;Integrated Security=True;"

#Jwt
3. dotnet user-secrets set "Jwt:Key" "yourKey"

4.  dotnet user-secrets set "Jwt:Login" "yourLogin" 

5. dotnet user-secrets set "Jwt:Password" "Dev@123"

#Cryptography

6. dotnet user-secrets set "Cryptography:Key" "yourKey"  
// sua key do cryptography dever ser divido por 8, ou seja, sujiro que tenha 8, 16, 24, 30... (digitos).