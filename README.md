# PullentiAPI

<div align="center">

[![GitHub Stars](https://img.shields.io/github/stars/Shooshpanius/Pullenti.Core?style=flat-square&logo=github&label=Stars&cacheSeconds=3600)](https://github.com/Shooshpanius/Pullenti.Core/stargazers)
[![GitHub Forks](https://img.shields.io/github/forks/Shooshpanius/Pullenti.Core?style=flat-square&logo=github&label=Forks&cacheSeconds=3600)](https://github.com/Shooshpanius/Pullenti.Core/forks)
[![GitHub Issues](https://img.shields.io/github/issues/Shooshpanius/Pullenti.Core?style=flat-square&logo=github&label=Issues)](https://github.com/Shooshpanius/Pullenti.Core/issues)

![.NET](https://img.shields.io/badge/.NET_8-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=flat-square&logo=csharp&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=flat-square&logo=docker&logoColor=white)
![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=flat-square&logo=swagger&logoColor=black)

</div>

REST API-сервис для морфологического анализа и распознавания именованных сущностей (NER) в русскоязычных текстах на основе библиотеки [Pullenti](https://pullenti.ru/).

---

## Содержание

1. [Обзор проекта](#обзор-проекта)
2. [Архитектура](#архитектура)
3. [Структура проекта](#структура-проекта)
4. [Технологический стек](#технологический-стек)
5. [Установка и запуск](#установка-и-запуск)
   - [Локальный запуск](#локальный-запуск)
   - [Запуск через Docker](#запуск-через-docker)
6. [Конфигурация](#конфигурация)
7. [API](#api)
   - [Аутентификация](#аутентификация)
   - [Эндпоинты](#эндпоинты)
   - [Swagger UI](#swagger-ui)
8. [Безопасность](#безопасность)
9. [Зависимости](#зависимости)

---

## Обзор проекта

**PullentiAPI** — это веб-сервис на базе ASP.NET Core (.NET 8), предоставляющий HTTP API для:

- **Морфологического анализа** текста (токенизация, лемматизация, определение частей речи);
- **Распознавания именованных сущностей** (Named Entity Recognition, NER) с использованием библиотеки Pullenti.Core.

Сервис защищён **Basic Authentication** и документируется через **Swagger / OpenAPI**.

---

## Архитектура

```
Клиент (HTTP)
    │
    ▼
BasicAuthenticationHandler  ←  IUserService / UserService
    │
    ▼
NerController  (POST /api/Ner/getNer)
    │
    ▼
Pullenti.Sdk  →  ProcessorService  →  MorphologyService
    │
    ▼
JSON-ответ (список MorphToken)
```

### Поток обработки запроса

1. Клиент отправляет `POST /api/Ner/getNer` с заголовком `Authorization: Basic <base64(user:password)>` и JSON-телом `{ "text": "..." }`.
2. `BasicAuthenticationHandler` декодирует заголовок и через `UserService` сверяет учётные данные с конфигурацией (`apiUser` / `apiPassword`).
3. При успешной аутентификации запрос передаётся в `NerController`.
4. Контроллер инициализирует SDK (`Pullenti.Sdk.InitializeAll()`), создаёт процессор и выполняет морфологический анализ.
5. Результат (список токенов `MorphToken`) сериализуется в JSON и возвращается клиенту.

---

## Структура проекта

```
Pullenti.Core.sln                  # Solution-файл Visual Studio
PullentiAPI/
├── Controllers/
│   ├── NerController.cs           # HTTP-контроллер NER/морфологии
│   └── MainConfig.cs              # Загрузка и хранение конфигурации приложения
├── Properties/
│   └── launchSettings.json        # Профили запуска (HTTP, IIS Express, Docker)
├── lib/
│   ├── Pullenti.Core.dll          # Основная библиотека Pullenti (NLP)
│   └── Pullenti.Core.deps.json    # Зависимости сборки
├── BasicAuthenticationHandler.cs  # Реализация Basic Auth (обработчик + сервис)
├── Program.cs                     # Точка входа, конфигурация DI и middleware
├── Dockerfile                     # Инструкция сборки Docker-образа
├── appsettings.json               # Базовые настройки приложения
└── PullentiAPI.csproj             # Файл проекта .NET
```

---

## Технологический стек

| Компонент             | Версия / Описание                         |
|-----------------------|-------------------------------------------|
| Платформа             | .NET 8.0                                  |
| Веб-фреймворк         | ASP.NET Core (Microsoft.NET.Sdk.Web)      |
| Документация API      | Swashbuckle.AspNetCore 6.6.2 (Swagger)    |
| Сериализация JSON     | Newtonsoft.Json 13.0.4                    |
| Аутентификация        | Basic Authentication (собственная реализация) |
| NLP-библиотека        | Pullenti.Core (подключена как DLL)        |
| Контейнеризация       | Docker (образ mcr.microsoft.com/dotnet/aspnet:8.0) |

---

## Установка и запуск

### Требования

- [.NET SDK 8.0+](https://dotnet.microsoft.com/download/dotnet/8.0)
- (опционально) [Docker](https://www.docker.com/)

### Локальный запуск

1. Клонируйте репозиторий:
   ```bash
   git clone <url-репозитория>
   cd Pullenti.Core
   ```

2. Создайте файл конфигурации для режима разработки `PullentiAPI/appsettings.Development.json` и задайте учётные данные API:
   ```json
   {
     "apiUser": "your_username",
     "apiPassword": "your_password"
   }
   ```

3. Запустите проект:
   ```bash
   cd PullentiAPI
   dotnet run
   ```

4. Сервис будет доступен по адресу `http://localhost:5090`.  
   Swagger UI откроется автоматически в браузере: `http://localhost:5090/swagger`.

### Запуск через Docker

1. Перейдите в директорию `PullentiAPI`:
   ```bash
   cd PullentiAPI
   ```

2. Соберите Docker-образ:
   ```bash
   docker build -t pullenti-api .
   ```

3. Запустите контейнер, передав учётные данные через переменные среды:
   ```bash
   docker run -d -p 8080:8080 \
     -e apiUser=your_username \
     -e apiPassword=your_password \
     --name pullenti-api \
     pullenti-api
   ```

4. Сервис будет доступен по адресу `http://localhost:8080`.

---

## Конфигурация

Приложение поддерживает два режима конфигурации:

| Режим        | Источник конфигурации                              |
|--------------|----------------------------------------------------|
| Разработка   | `PullentiAPI/appsettings.Development.json`         |
| Продакшн     | Переменные среды операционной системы / Docker     |

### Параметры конфигурации

| Ключ          | Описание                                   | Пример         |
|---------------|--------------------------------------------|----------------|
| `apiUser`     | Имя пользователя для Basic Authentication  | `admin`        |
| `apiPassword` | Пароль для Basic Authentication            | `secret123`    |

> **Важно:** Файл `appsettings.Development.json` внесён в `.gitignore` и не должен попадать в систему контроля версий. Никогда не добавляйте учётные данные в репозиторий.

---

## API

### Аутентификация

Все эндпоинты защищены **Basic Authentication**.

Для выполнения запроса необходимо передать заголовок:
```
Authorization: Basic <base64(username:password)>
```

Пример кодирования (bash):
```bash
echo -n "admin:secret123" | base64
# YWRtaW46c2VjcmV0MTIz
```

---

### Эндпоинты

#### `POST /api/Ner/getNer`

Выполняет морфологический анализ переданного текста и возвращает список токенов с морфологическими характеристиками.

**Заголовки запроса:**

| Заголовок       | Значение                  | Обязательный |
|-----------------|---------------------------|--------------|
| `Authorization` | `Basic <base64 credentials>` | Да        |
| `Content-Type`  | `application/json`        | Да           |

**Тело запроса (JSON):**

```json
{
  "text": "Москва — столица России."
}
```

| Поле   | Тип    | Описание                     |
|--------|--------|------------------------------|
| `text` | string | Текст для анализа (обязательно) |

**Ответ (200 OK):**

Массив объектов `MorphToken` в формате JSON. Каждый токен содержит информацию о словоформе, лемме, части речи и других морфологических характеристиках, предоставляемых библиотекой Pullenti.

```json
[
  {
    // Морфологические характеристики токена (структура определяется Pullenti.Core)
  },
  ...
]
```

**Пример запроса (curl):**

```bash
curl -X POST "http://localhost:5090/api/Ner/getNer" \
     -H "Authorization: Basic YWRtaW46c2VjcmV0MTIz" \
     -H "Content-Type: application/json" \
     -d '{"text": "Москва — столица России."}'
```

**Коды ответов:**

| Код  | Описание                                      |
|------|-----------------------------------------------|
| 200  | Успешный анализ, возвращается JSON с токенами |
| 401  | Неверные или отсутствующие учётные данные     |

---

### Swagger UI

В режиме разработки (`ASPNETCORE_ENVIRONMENT=Development`) Swagger UI доступен по адресу:

```
http://localhost:5090/swagger
```

Swagger поддерживает Basic Authentication: нажмите кнопку **Authorize** и введите имя пользователя и пароль для тестирования эндпоинтов прямо из браузера.

---

## Безопасность

- **Basic Authentication**: учётные данные передаются в заголовке в формате Base64. Для защиты от перехвата рекомендуется использовать **HTTPS** в продакшн-окружении.
- **Конфигурация**: в продакшне учётные данные передаются через переменные среды, что исключает их попадание в код или конфигурационные файлы репозитория.
- **Swagger**: в продакшн-режиме (`ASPNETCORE_ENVIRONMENT != Development`) Swagger UI отключён автоматически.

---

## Зависимости

### NuGet-пакеты

| Пакет                                        | Версия  | Назначение                         |
|----------------------------------------------|---------|------------------------------------|
| `Swashbuckle.AspNetCore`                     | 6.6.2   | Генерация Swagger / OpenAPI документации |
| `Newtonsoft.Json`                            | 13.0.4  | Сериализация/десериализация JSON   |
| `Microsoft.Extensions.ApiDescription.Client`| 7.0.2   | Клиент описания API (NSwag/Swagger)|
| `NSwag.ApiDescription.Client`               | 13.18.2 | Генерация клиентского кода по OpenAPI |
| `Microsoft.VisualStudio.Azure.Containers.Tools.Targets` | 1.22.1 | Поддержка Docker в Visual Studio |

### Локальные сборки

| Файл                       | Назначение                                      |
|----------------------------|-------------------------------------------------|
| `lib/Pullenti.Core.dll`    | Основная NLP-библиотека Pullenti для морфологического анализа и NER |