# <PROJECT_NAME>

## Importação e uso da biblioteca

### Importação pelo GitHub

A forma mais simples de importar o pacote e garantir sempre ter uma versão atualizada é por meio da importação pela url do github.

Para isso abra o *Package Manager* e na opção de *Add package from git URL* adicione a seguinte url:

```
https://github.com/BGS-UnityFoundation/<PROJECT_REPO>.git?path=/Assets/<PROJECT_NAME>
```

Por meio desse link o Package Manager da Unity irá baixar o a pasta selecionada na raiz do projeto.

### Importação por .unitypackage

O pacote `<PROJECT_PACKAGE>.unitypackage` pode ser baixado e importado no Editor da Unity.

Depois de importado alguns pacotes devem ser adicionados ao `manisfest.json` por serem dependências desses pacote.

```json
{
    "com.unity.inputsystem": "1.4.4",
    "com.unity.textmeshpro": "3.0.6"
}
```