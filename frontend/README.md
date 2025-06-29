# Freelancer Management System

A full-stack, clean-architecture project for managing freelancers, featuring a .NET 8 RESTful API with SQLite and a modern HTML/CSS/JavaScript frontend. Includes CRUD, search, archive/unarchive, validation, pagination, error handling, and a beautiful UI.

---

## 🚀 Features
- **RESTful API**: CRUD, search, archive/unarchive
- **Frontend**: Responsive, modern, pure HTML/CSS/JS (no build tools)
- **Validation**: Server-side (FluentValidation) and client-side
- **Pagination**: On both list and search
- **Soft Delete**: Archive/unarchive without data loss
- **Error Handling**: Global exception middleware, user-friendly frontend notifications
- **Testing**: xUnit and Moq for backend unit tests

---

## 🏗️ Architecture
- **Domain**: Core models (Freelancer, Skillset, Hobby)
- **Application**: Business logic, DTOs, validation
- **Infrastructure**: EF Core, SQLite, repositories
- **API**: ASP.NET Core Web API, controllers, DI
- **Frontend**: Pure HTML/CSS/JS, no dependencies
- **Tests**: xUnit, Moq

```
/ (solution root)
  CDN.Freelancer.Domain/         # Domain models
  CDN.Freelancer.Application/    # Services, DTOs, validation
  CDN.Freelancer.Infrastructure/ # EF Core, SQLite, repositories
  CDN.Freelancer.Api/            # ASP.NET Core Web API
  CDN.Freelancer.Tests/          # Unit tests
  frontend/                      # HTML/CSS/JS frontend
```


## ⚡ Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Modern web browser
- (Optional) Python 3 for serving frontend: `python -m http.server 8080`

### 1. Clone the Repository
```sh
git clone https://github.com/yourusername/your-repo-name.git
cd your-repo-name
```

### 2. Run the API
```sh
cd CDN.Freelancer.Api
dotnet run
```
API runs at: `http://localhost:5016`

### 3. Run the Frontend
```sh
cd ../frontend
python -m http.server 8080
```
Frontend at: `http://localhost:8080`

---

## 🧪 Running Tests
```sh
cd ../CDN.Freelancer.Tests
dotnet test
```

---

## 📚 API Endpoints
- `GET    /api/freelancers`           - List (paginated)
- `GET    /api/freelancers/search`    - Search (paginated)
- `POST   /api/freelancers`           - Create
- `GET    /api/freelancers/{id}`      - Get by ID
- `PUT    /api/freelancers/{id}`      - Update
- `DELETE /api/freelancers/{id}`      - Delete
- `PATCH  /api/freelancers/{id}/archive`   - Archive
- `PATCH  /api/freelancers/{id}/unarchive` - Unarchive

See Swagger UI at `http://localhost:5016/swagger` for details.

---

## 🌐 Deployment
- **API**: Deploy to Azure App Service, AWS, or any .NET host. For production, use a real DB (not SQLite).
- **Frontend**: Deploy static files to GitHub Pages, Netlify, Vercel, or serve from the API.

---

## 📝 Customization
- **API URL**: Change `API_BASE_URL` in `frontend/script.js` if needed.
- **Styling**: Edit `frontend/styles.css` for custom themes.
- **Validation**: Extend FluentValidation rules in Application layer.
- **Features**: Add more endpoints, authentication, etc.

---

## 🐞 Troubleshooting
- **CORS**: Ensure API allows requests from frontend origin.
- **Port Conflicts**: Change ports in launch settings if needed.
- **DB Locked**: Stop API before deleting or replacing the SQLite DB.

---

## 📄 License
MIT (see LICENSE file)

---

## 🙏 Credits
- .NET, EF Core, FluentValidation, xUnit, Moq
- Font Awesome (icons)

---

## 🎉 Demo Tips
- Show archiving/unarchiving with the UI toggle
- Use Swagger to show API endpoints
- Run tests to prove code quality
- Highlight clean architecture and separation of concerns

---

## 🚦 Demo

To demonstrate the main features:
1. **Start the API** (`dotnet run` in CDN.Freelancer.Api)
2. **Start the frontend** (`python -m http.server 8080` in frontend/)
3. **Open the frontend** at http://localhost:8080
4. **Create, archive, unarchive, and search freelancers** using the UI
5. **Show validation and error handling** in the 'Test Validation' tab
6. **Show API endpoints** in Swagger at http://localhost:5016/swagger

For a detailed walkthrough and screenshots, see [demo.md](demo.md).

---

