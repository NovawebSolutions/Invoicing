## Novaweb Invoicing

Projects used to create an invoicing pipeline.

### TogglImport

Extract data from [Toggl](http://www.toggl.com) and push their content in an Azure blob Storage

### TogglModel

Model representing the Toggl API DTO.

### Invoicing.Model

Model used by the Invoicing application.

### TransformRawData

Application that reads the Toggl raw DTO and converts them into the invoicing model and upload them to Azure Tables. 
