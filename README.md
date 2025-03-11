## Требования
- .NET 8.0 SDK
- IDE (например, Visual Studio, JetBrains Rider или Visual Studio Code)
- curl

## Установка
1. Клонируйте репозиторий.
2. Откройте проект в Visual Studio или JetBrains Rider.
3. Запустите проект.

## Использование
- Загрузить данные из файла:
  Используйте curl:
  ```bash
  curl -X POST -F "file=@path/to/your/file.txt" https://localhost:5001/api/AdPlatforms/upload
  ```
- Поиск рекламных площадок по локации:
  ```bash
  curl -X GET "https://localhost:5001/api/AdPlatforms/search?location=/ru/svrd/revda"
  ```

  ## Формат файла
  ```
  Яндекс.Директ:/ru
  Ревдинский рабочий:/ru/svrd/revda,/ru/svrd/pervik
  Газета уральских москвичей:/ru/msk,/ru/permobl,/ru/chelobl
  Крутая реклама:/ru/svrd
  ```

  ## Ошибка с которой я столкнулся
  (35) schannel: failed to receive handshake, SSL/TLS connection failed
  Выполнил
  ```bash
  dotnet dev-certs https --trust
  ```
