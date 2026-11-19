# ENTREGA - Projecte II Kahoot

## Estado
Todo el código está hecho y compila (comprobado con el compilador de Unity). **No lo he podido ejecutar**: al abrir Unity,
mira que la Console no tenga errores rojos y juega una partida.

La UI está montada dentro de las 9 escenas y de los prefabs (sin generar nada por código); los scripts son los del PDF y los assets (sprites, audio, prefabs, colores) se asignan en el Inspector.

## Qué entregar (3 partes + documento)
| Parte | Dónde está | Falta |
|---|---|---|
| 1. Mockups | `Documentation/Mockups/*.png` (9 pantallas) | Nada |
| 2. Esquema de navegación (Draw.io) | `Documentation/Navigation/` (.drawio, .pdf, .png) | Nada |
| 3. GitHub: proyecto + Build en Releases | `.gitignore` y `README.md` ya listos | Subir, build y release (abajo) |
| 4. Documento explicando la rúbrica | - | Escribirlo (abajo) |

## Pasos que te quedan
1. **Unity**: abre el proyecto, juega `MainMenu`. Prueba: jugar, ver Highscores, crear un Kahoot y verlo en el selector.
2. **Pruebas de errores** (sirven de evidencia): copia los archivos de `Documentation/TestData/Kahoots` a
   `%USERPROFILE%\AppData\LocalLow\DefaultCompany\Kahoot Challenge\Kahoots`, abre el selector y mira `ERROR REPORTS`.
   Para XML roto copia `TestData/Leaderboards/UnityBasics_CORRUPT.xml` a la carpeta `Leaderboards` como `UnityBasics.xml`.
3. **Capturas** de tu app en `Documentation/Screenshots`: menú, selector, partida, acierto, fallo, resultados, ranking,
   creador, lista de informes, visor, carpeta con JSON, carpeta con XML, carpeta con informes.
4. **Build**: File > Build Profiles > Windows, escenas añadidas con MainMenu primera, Company Name = tu nombre,
   Build en carpeta `Build/` como `KahootChallenge.exe`. Comprime la carpeta entera en `KahootChallenge_Windows_v1.0.zip`.
5. **GitHub**: repo público `Projecte-II-Kahoot`, sube todo (Library ya está ignorada), y en Releases crea la `v1.0.0` con el ZIP.
6. **Documento PDF** `ProjecteII_Kahoot_Documentation.pdf` con estas secciones, cada una con captura + fragmento de código:

| Apartado | Evidencia | Código / clase |
|---|---|---|
| JSON | Captura selector + ejemplo JSON | `KahootManager.GetAvailableKahoots / LoadKahoot` |
| Excepciones JSON | Un informe .txt real | `KahootManager.ValidateKahoot`, `ExceptionManager.Log` |
| XML | Ejemplo con varios usuarios | `LeaderboardManager.AddResult` |
| Excepciones XML | XML roto + informe | `LeaderboardUI.ShowSelectedLeaderboard` (catch) |
| Leaderboards | Dos rankings distintos | `LeaderboardUI` |
| Gameplay | Partida, acierto, fallo | `KahootGameController` (temporizador y puntos) |
| Creador | Varias preguntas + JSON generado | `KahootCreatorController`, `QuestionEditorItem` |
| Informes | Lista y visor | `ReportsListUI`, `ReportViewerUI` |
| Navegación | Diagrama + prueba con ratón/teclado/mando | `SceneLoader`, esquema Draw.io |
| SOLID | Tabla de responsabilidades | ver abajo |
| Interfaz | Mockups + Gestalt | similitud (mismos botones/colores), proximidad (tarjetas), feedback (verde/rojo, tiempo en rojo) |

**SOLID (para copiar):** S: cada clase una función (`LeaderboardManager` solo rankings, `ExceptionManager` solo informes,
`KahootManager` solo cuestionarios). O: se añaden Kahoots JSON sin tocar el juego. L: no hay herencia propia, no se aplica.
I: el visor solo usa `ReadReport`. D: la UI no lee archivos, delega en los managers.

## Checklist final
- [ ] Sin errores rojos en Console
- [ ] Capturas hechas
- [ ] Build probada fuera de Unity
- [ ] Repo en GitHub + Release con el ZIP
- [ ] PDF del documento
- [ ] Enlaces comprobados (rellena el del README)
