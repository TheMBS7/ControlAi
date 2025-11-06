# ControlAi

## Visão Geral

Um projeto próprio que estou desenvolvendo para auxiliar no controle de gastos mensais, permitindo a criação de movimentos fixos de entradas e saídas, que são gerados automaticamente em seus respectivos períodos, além de lançamentos avulsos dentro de cada mês.
Todas as movimentações podem ser classificadas por Pessoas e Categorias, garantindo uma organização financeira de forma simples e organizada.

## Tecnologias

- **Backend**: C# (.NET 8, ASP.NET Core Web API), Entity Framework Core
- **Frontend**: Next.js, React, Tailwind CSS 

## Funcionalidades

- **Cadastro de categorias**: Criação e gerenciamento de categorias para os movimentos.  
- **Cadastro de pessoas**: Associação de lançamentos a pessoas específicas.  
- **Controle de lançamentos**: Registro de entradas e saídas.   
- **Extratos**: Visualização de lançamentos mensais e totais consolidados.  

## Fluxo de Uso

1. O usuário cadastra **categorias** e **pessoas** no sistema.  
2. Em seguida, registra **entradas e saídas fixas** associando a categoria e a pessoa correspondente.  
3. Os **movimentos fixas** são lançadas automaticamente para cada mês, facilitando a projeção dos meses.
4. O sistema mantém o **histórico financeiro mensal**, permitindo adição, edição ou remocão de movimentos em cada mês.

---

Desenvolvido por [Matheus Branco](https://github.com/matheusbranco).
