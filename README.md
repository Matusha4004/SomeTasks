## Установка
1. Клонируйте репозиторий.
2. Откройте проект в Visual Studio или JetBrains Rider.
3. Запустите проект.

## Использование
- Загрузить данные из файла:
  Используйте Postman или curl:
  ```bash
  curl -X POST -F "file=@path/to/your/file.txt" https://localhost:5001/api/AdPlatforms/upload
- Поиск рекламных площадок по локации:

```bash
curl -X GET "https://localhost:5001/api/AdPlatforms/search?location=/ru/svrd/revda"
