# Kahoot Challenge

Aplicación estilo Kahoot desarrollada en **Unity 6000.3.14f1** para el *Projecte II* de DAM 2n.
Lee cuestionarios desde archivos JSON, los muestra en secuencia y con tiempo limitado, guarda las
puntuaciones en archivos XML (uno por cuestionario) y permite crear nuevos Kahoots sin salir de la
aplicación. Los errores se registran en informes de texto que se pueden consultar desde la propia app.

> Desarrollador: Aaron Hernández Moreno

## Funcionalidades

- Menú principal con navegación por **ratón, teclado y gamepad** (Esc / botón B para volver).
- **Selector de Kahoots** dinámico: 4 cuestionarios por defecto (`Assets/Resources/DefaultKahoots`) + los
  `.json` que se copien a `persistentDataPath/Kahoots`.
- **Partida**: preguntas secuenciales, 4 respuestas, temporizador por pregunta, puntuación proporcional
  al tiempo restante, indicación de la respuesta correcta y feedback visual y sonoro.
- **Resultados**: puntuación, porcentaje de aciertos, correctas/incorrectas, tiempo total y posición en el ranking.
- **Highscores** accesibles sin jugar: un ranking XML independiente por Kahoot.
- **Creador de Kahoots**: número variable de preguntas, enunciados, 4 respuestas, respuesta correcta,
  duración por pregunta y validación completa. El cuestionario aparece al instante en el selector.
- **Informes de errores** `.txt` con fecha y hora en el nombre, listado y visor completo dentro de la app.

## Estructura

```
Assets/
├── Art/          Sprites (logo y PNG de la UI), Materials y Shaders (fondo)
├── Audio/        Music y SFX
├── Prefabs/      KahootCard, LeaderboardEntry, ReportButton, QuestionEditorItem
├── Resources/    DefaultKahoots (JSON por defecto)
├── Scenes/       9 escenas (MainMenu, KahootSelector, KahootGame, Results, Highscores,
│                 KahootCreator, ReportsList, ReportViewer, About)
└── Scripts/      Data, Managers, Gameplay, Creator, UI, Utilities
Documentation/
├── Mockups/       Mockups de las 9 pantallas (PNG)
├── Navigation/    Esquema de navegación (.drawio + PDF + PNG)
├── Screenshots/   Capturas reales de la aplicación
└── TestData/      JSON y XML de prueba (correctos y defectuosos)
```

## Cómo ejecutar

1. Abrir el proyecto con **Unity 6000.3.14f1**.
2. Abrir la escena `Assets/Scenes/MainMenu.unity` y pulsar *Play*.
3. Para jugar sin Unity, descargar la build desde **Releases**, descomprimir el ZIP y ejecutar `KahootChallenge.exe`.

Los datos del usuario se guardan en `Application.persistentDataPath`
(`%USERPROFILE%\AppData\LocalLow\<Company>\Kahoot Challenge\`):

| Carpeta | Contenido |
|---|---|
| `Kahoots/` | JSON de cuestionarios creados o copiados a mano |
| `Leaderboards/` | Un `.xml` de resultados por cada Kahoot |
| `ErrorReports/` | Informes `Exception_yyyyMMdd_HHmmss_fff.txt` |

## Formato JSON de un Kahoot

```json
{
  "title": "Mi Kahoot",
  "description": "Opcional",
  "questions": [
    {
      "questionText": "¿Capital de España?",
      "answers": ["Barcelona", "Valencia", "Madrid", "Sevilla"],
      "correctAnswerIndex": 2,
      "durationSeconds": 15
    }
  ]
}
```

## Enlaces

- Build de Windows: *(añadir enlace a la Release)*
