# Use the official .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Set the working directory in the container
WORKDIR /src

# Copy only the necessary project files into the container
COPY ChessAPI/ChessAPI.csproj ChessAPI/
COPY ChessBot/ChessBot.csproj ChessBot/
COPY ChessLogic/ChessLogic.csproj ChessLogic/

# Restore dependencies
RUN dotnet restore "ChessAPI/ChessAPI.csproj"

# Copy the rest of the application files for all projects
COPY . .

# Build the project
WORKDIR "/src/ChessAPI"
RUN dotnet build "ChessAPI.csproj" -c Release -o /app/build

# Final stage to run the application
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/build .

# Expose the port that the API listens on
# Set the environment variable for Docker to listen on port 80
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

# Set the entry point for the application
ENTRYPOINT ["dotnet", "ChessAPI.dll"]




