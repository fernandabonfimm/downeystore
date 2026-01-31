# RestaurantQueue

## Sobre o Projeto

Este projeto é inspirado na loja de Downey na Califórnia, que permanece praticamente inalterada desde sua inauguração em 1953. O objetivo é implementar um sistema de gerenciamento de fila de pedidos de restaurante utilizando boas práticas de desenvolvimento.

## Áreas da Cozinha

O sistema gerencia pedidos através das seguintes estações de trabalho:
- **Grelha**: Preparação das carnes (hambúrgueres)
- **Salada**: Preparação dos ingredientes frescos (tomate, picles)
- **Fritas**: Preparação de batatas fritas
- **Bebida**: Preparação das bebidas

Os **Lanches** (como Big Mac) são produtos finais que passam por várias estações até ficarem prontos.

## Boas Práticas Implementadas

O projeto demonstra a aplicação de:
- **SOLID**: Princípios de design orientado a objetos
- **Imutabilidade**: Prevenção de efeitos colaterais indesejados
- **Injeção de Dependência**: Baixo acoplamento e alta testabilidade
- **Testes Unitários**: Garantia de qualidade com xUnit
- **Separação de Responsabilidades**: Organização clara em camadas (Controllers, Services, Models)
- **Prevenção de Memory Leak**: Gerenciamento adequado de recursos na fila de pedidos

## Fluxo de Pedidos

O sistema gerencia o fluxo de pedidos desde a entrada até o consumidor final, passando pelas diferentes áreas da cozinha de forma eficiente e organizada.

### Arquitetura do Sistema

O **DowneyStoreController** é o ponto central de entrada para todas as operações. Ele é responsável por:
- Receber requisições de pedidos
- Criar consumidores (Consumer) automaticamente com ID randomico
- Adicionar produtos (Product) ao pedido
- Processar pagamentos (Payment)
- Gerar e armazenar o pedido completo (Order)

Todas as operações passam pelo **OrderService**, que contém a lógica de negócio e interage com o **Storage** (simulando banco de dados em memória).

## Tecnologias

- .NET 10
- ASP.NET Core Web API
- xUnit para testes

## Estrutura do Projeto

```
RestaurantQueue/
├── Controllers/      # Controladores da API
├── Services/         # Lógica de negócio
├── Models/           # Modelos de dados
│   ├── DTOs/         # Data Transfer Objects
│   ├── Consumer.cs
│   ├── Order.cs
│   ├── Product.cs
│   └── Payment.cs
├── Storage/          # Armazenamento em memória
└── Program.cs        # Configuração da aplicação

RestaurantQueue.Tests/
├── Controllers/      # Testes dos controladores
└── Services/         # Testes dos serviços
```

## API Endpoints

### DowneyStore (Ponto Central de Pedidos)

Todos os endpoints são acessados através do controlador DowneyStore, que centraliza as operações de criação de pedidos, produtos e consumidores.

**POST** `/api/restaurant/downeystore/order` - Criar novo pedido
```json
{
  "consumerName": "Fernanda Bonfim Santos",
  "paymentMethod": "Debit Card",
  "productIds": [1, 6, 8]
}
```

**Exemplo de Resposta:**
```json
{
  "orderId": 1,
  "consumerName": "Fernanda Bonfim Santos",
  "products": [
    {"id": 1, "name": "Big Mac", "price": 5.99, "category": "Lanche"},
    {"id": 6, "name": "Batata Media", "price": 2.49, "category": "Fritas"},
    {"id": 8, "name": "Refil Coca Cola", "price": 1.99, "category": "Bebida"}
  ],
  "totalAmount": 10.47,
  "paymentMethod": "Debit Card",
  "createdAt": "2026-01-31T10:00:00Z",
  "status": "Pending"
}
```

**GET** `/api/restaurant/downeystore/order/{id}` - Obter pedido por ID

**GET** `/api/restaurant/downeystore/orders` - Listar todos os pedidos

**GET** `/api/restaurant/downeystore/products` - Listar todos os produtos disponíveis

**POST** `/api/restaurant/downeystore/product` - Criar novo produto
```json
{
  "name": "McChicken",
  "price": 4.99,
  "category": "Lanche"
}
```
Categorias válidas: `Lanche`, `Grelha`, `Salada`, `Fritas`, `Bebida`

Resposta de sucesso:
```json
{
  "id": "guid-do-produto",
  "name": "McChicken",
  "price": 4.99,
  "category": "Grelha",
  "createdAt": "2026-01-30T20:50:00Z"
}
```

**POST** `/api/restaurant/downeystore/preparation/grill` - Marcar grelha como concluída
```json
{
  "orderId": 1,
  "station": "grill"
}
```

**POST** `/api/restaurant/downeystore/preparation/salad` - Marcar salada como concluída
```json
{
  "orderId": 1,
  "station": "salad"
}
```

**POST** `/api/restaurant/downeystore/preparation/fries` - Marcar fritas como concluída
```json
{
  "orderId": 1,
  "station": "fries"
}
```

**POST** `/api/restaurant/downeystore/preparation/refill` - Marcar refil como concluído
```json
{
  "orderId": 1,
  "station": "refill"
}
```

**GET** `/api/restaurant/downeystore/preparation/status/{orderId}` - Obter status atual de preparação do pedido

**GET** `/api/restaurant/downeystore/preparation/history/{orderId}` - Obter histórico completo de preparação do pedido

**POST** `/api/restaurant/downeystore/deliver` - Entregar pedido ao cliente
```json
{
  "orderId": 1
}
```
Resposta de sucesso:
```json
{
  "orderId": 1,
  "isReady": true,
  "message": "Enjoy your meal!",
  "deliveredAt": "2026-01-30T20:45:00Z"
}
```

### Produtos Pré-cadastrados

O sistema já vem com os seguintes produtos cadastrados (IDs fixos):

**ID 1 - Lanches:**
- Big Mac - $5.99

**ID 2 - Grelha (Carnes):**
- Hamburguer - $2.50

**IDs 3-5 - Salada (Ingredientes):**
- 3: Alface - $0.50
- 4: Tomate - $0.50
- 5: Picles - $0.30

**IDs 6-7 - Fritas:**
- 6: Batata Media - $2.49
- 7: Batata Grande - $3.49

**ID 8 - Bebidas:**
- Refil Coca Cola - $1.99

## Executar o Projeto

### Via Terminal
```bash
cd RestaurantQueue
dotnet run
```

### Via Visual Studio
Pressione **F5** (com debug) ou **Ctrl+F5** (sem debug)

### Via Visual Studio Code
Pressione **F5** ou use o terminal integrado com `dotnet run`

---

## Acessar a API

Após executar o projeto, a API estará disponível em:
- **Swagger UI (Recomendado)**: `http://localhost:5000` ou `https://localhost:5001`
- **Endpoint Hello**: `http://localhost:5000/hello`

### Swagger UI - Interface Visual Interativa

O projeto inclui **Swagger UI** que permite:
- ✅ Visualizar todos os endpoints disponíveis
- ✅ Testar cada endpoint diretamente no navegador
- ✅ Ver a documentação automática da API
- ✅ Validar requests e responses
- ✅ Copiar exemplos de código

**Como usar:**
1. Execute o projeto (`dotnet run`)
2. Abra o navegador em `http://localhost:5000`
3. A interface do Swagger será exibida automaticamente
4. Clique em qualquer endpoint para expandir e testar

### Testar via cURL (Alternativa)

```bash
# Listar produtos
curl http://localhost:5000/api/restaurant/downeystore/products

# Criar produto
curl -X POST http://localhost:5000/api/restaurant/downeystore/product \
  -H "Content-Type: application/json" \
  -d '{"name":"McChicken","price":4.99,"category":"Grelha"}'

# Criar pedido (primeiro pegue os IDs dos produtos)
curl -X POST http://localhost:5000/api/restaurant/downeystore/order \
  -H "Content-Type: application/json" \
  -d '{"consumerName":"John Doe","paymentMethod":"Cash","productIds":["guid-do-produto"]}'
```

## Executar os Testes

```bash
dotnet test
```
