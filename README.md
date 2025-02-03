# MailBot

Uma biblioteca .NET para envio de emails com suporte a templates.

## 🚀 Funcionalidades

- ✉️ Envio de emails via SMTP
- 📝 Suporte a templates de email
- 🔄 Substituição de variáveis dinâmicas
- 📎 Suporte a anexos
- 🔒 Configuração segura via arquivo de configuração
- 📋 Logging detalhado com Serilog

## 📋 Pré-requisitos

- .NET 8.0 ou superior
- Servidor SMTP configurado

## ⚙️ Configuração

1. Clone o repositório
2. Copie o arquivo `appsettings.example.json` para `appsettings.json`
3. Configure suas credenciais SMTP:

```json
{
    "EmailSettings": {
        "SmtpServer": "seu.smtp.com",
        "Port": 587,
        "Username": "seu@email.com",
        "Password": "suasenha",
        "FromName": "MailBot",
        "FromAddress": "noreply@seudominio.com"
    }
}
```

## 🔧 Uso

### Envio Simples

```csharp
var emailMessage = new EmailMessage
{
        To = "destinatario@email.com",
        Subject = "Assunto",
        Body = "Conteúdo do email",
        IsHtml = false
};

await emailSender.SendEmailAsync(emailMessage);
```

### Usando Templates

```csharp
var variables = new Dictionary<string, string>
{
        { "UserName", "João" }
};

await emailSender.SendEmailWithTemplateAsync(
        "destinatario@email.com",
        "welcome",
        variables
);
```

## 🧪 Testes

O projeto inclui testes unitários e de integração usando xUnit. Para executar:

```bash
dotnet test
```

## 📚 Estrutura do Projeto

- `MailBot/` - Código fonte principal
    - `MailService/` - Serviços de email
    - `Models/` - Classes de modelo
- `Tests/` - Testes unitários e de integração

## 🛠️ Tecnologias

- .NET 8.0
- MailKit
- Serilog
- xUnit
- Moq

## 📄 Licença

Este projeto está sob a licença MIT - veja o arquivo [LICENSE](LICENSE) para detalhes.

## ✨ Contribuindo

1. Fork o projeto
2. Crie sua Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a Branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request