# 🛒 AleCell — TCC Sistema Web

Loja de celulares Apple & Xiaomi com back-end completo em **.NET 10**, banco **MySQL (XAMPP)** e arquitetura **API + MVC**.

---

## 📁 Estrutura do Projeto

```
AleCell/
├── AleCell.API/          ← API RESTful (back-end + banco)
│   ├── Controllers/      ← AuthController, CategoriasController, ProdutosController
│   ├── Data/             ← AppDbContext (EF Core + Seeds)
│   ├── DTOs/             ← Objetos de transferência de dados
│   ├── Helpers/          ← Tradução de erros do Identity
│   ├── Middleware/        ← Tratamento global de erros
│   ├── Models/           ← Categoria, Produto, Usuario
│   └── Services/         ← FileService, JwtService, AuthService
│
├── AleCell.UI/           ← Front-end MVC (interface do usuário)
│   ├── Controllers/      ← Home, Auth, Admin, Categorias, Produtos
│   ├── DTOs/             ← DTOs para consumo da API
│   ├── Middleware/        ← SessionAuth middleware
│   ├── Services/         ← Serviços que consomem a API
│   ├── ViewModels/       ← ViewModels de cada formulário
│   ├── Views/            ← Razor Views (Home, Auth, Admin, etc.)
│   └── wwwroot/          ← CSS, JS (dark neon AleCell)
│
└── AleCell.sln           ← Solução Visual Studio
```

---

## ⚙️ Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [XAMPP](https://www.apachefriends.org/) com MySQL rodando na porta **3306**
- Visual Studio 2022 (ou VS Code)

---

## 🚀 Como Executar

### 1. Iniciar o MySQL no XAMPP
Abra o Painel do XAMPP e clique em **Start** no módulo **MySQL**.

---

### 2. Configurar a string de conexão

No arquivo `AleCell.API/appsettings.json`, ajuste conforme necessário:

```json
"ConnectionStrings": {
  "Conexao": "server=localhost;port=3306;database=alecell_db;uid=root;pwd=''"
}
```
> Se você definiu uma senha para o root, substitua `''` pela senha.

---

### 3. Rodar a API

```bash
cd AleCell.API
dotnet run
```

A API sobe em `http://localhost:5058`. O banco é criado e populado automaticamente.  
Acesse `http://localhost:5058` para abrir o Swagger e testar os endpoints.

---

### 4. Rodar o projeto UI

Em outro terminal:

```bash
cd AleCell.UI
dotnet run
```

A interface sobe em `http://localhost:5000` (ou porta indicada no console).

---

### 5. Acessar o sistema

| URL | Descrição |
|-----|-----------|
| `http://localhost:5000` | Loja pública (Home) |
| `http://localhost:5000/Home/Catalogo` | Catálogo de produtos |
| `http://localhost:5000/Auth/Login` | Login de cliente |
| `http://localhost:5000/Auth/Registro` | Cadastro de cliente |
| **`http://localhost:5000/admin`** | ⚙️ **Acesso à área administrativa** |

---

## 🔐 Credenciais de Administrador (seed)

| Campo | Valor |
|-------|-------|
| **E-mail** | `admin@alecell.com.br` |
| **Senha** | `Admin@123` |

---

## 🗺️ Rotas da API

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| POST | `/api/auth/register` | Cadastrar usuário |
| POST | `/api/auth/login` | Login + JWT |
| GET | `/api/auth/me` | Dados do usuário logado |
| GET | `/api/categorias` | Listar categorias |
| POST | `/api/categorias` | Criar categoria |
| PUT | `/api/categorias/{id}` | Editar categoria |
| DELETE | `/api/categorias/{id}` | Excluir categoria |
| GET | `/api/produtos` | Listar todos os produtos |
| GET | `/api/produtos/destaque` | Produtos em destaque |
| GET | `/api/produtos/categoria/{id}` | Produtos por categoria |
| POST | `/api/produtos` | Criar produto |
| PUT | `/api/produtos/{id}` | Editar produto |
| DELETE | `/api/produtos/{id}` | Excluir produto |

---

## 📌 Notas Importantes

- O acesso à área administrativa é feito **exclusivamente por URL**: `http://localhost:5000/admin`
- Não há botão de acesso admin na interface pública
- Imagens são salvas em `AleCell.API/wwwroot/img/` (categorias, produtos, usuários)
- O banco `alecell_db` é criado automaticamente pelo EF Core no primeiro `dotnet run` da API
