## Konfiguration

Um sicherzustellen, dass dein lokales LLM von deinem Spiel erreicht werden kann, müssen bestimmte Umgebungsvariablen gesetzt werden.

### IP-Adresse und Ursprünge anpassen

Setze die folgenden Umgebungsvariablen, damit ollama auf allen Netzwerkschnittstellen (0.0.0.0) lauscht und Verbindungen von allen Ursprüngen erlaubt:

```bash
launchctl setenv OLLAMA_HOST "0.0.0.0"
launchctl setenv OLLAMA_ORIGINS "*"
```

Diese Befehle passen die Einstellungen deines Systems an, sodass das LLM auch über das lokale Netzwerk erreichbar ist.

## Starten des LLM

Um das LLM zu starten, führe die folgenden Befehle im Terminal aus:

1. **Bestehende ollama-Prozesse beenden:**  
   Damit sichergestellt ist, dass keine alten Instanzen laufen, beende alle laufenden ollama-Prozesse.
   
   ```bash
   pkill ollama
   ```

2. **Lade das LLM mit der angepassten IP-Konfiguration:**  
   Starte ollama im Serve-Modus und achte darauf, dass die Umgebungsvariable `OLLAMA_HOST` korrekt verwendet wird.
   
   ```bash
   OLLAMA_HOST=0.0.0.0 ollama serve &
   ```
   
   Drücke anschließend die Eingabetaste, falls du dazu aufgefordert wirst. Das `&` sorgt dafür, dass der Prozess im Hintergrund läuft und dein Terminal weiterhin verfügbar bleibt.

## Verbindung mit dem Spiel

Dein Spiel wird über die IP-Adresse `0.0.0.0` (bzw. die konfigurierte Adresse) auf das LLM zugreifen. Stelle sicher, dass in deinem Spiel die URL korrekt konfiguriert ist (z. B. `http://127.0.0.1:11434/api/generate` oder die entsprechende Adresse deines lokalen PCs). Dadurch kannst du KI-Antworten in Echtzeit generieren und im Dialogfeld deines Spiels anzeigen.

## Fehlerbehebung

- **Verbindungsprobleme:**
  - Überprüfe, ob ollama tatsächlich auf `0.0.0.0` lauscht.
  - Stelle sicher, dass die Firewall deines Systems Verbindungen auf dem entsprechenden Port (z. B. 11434) zulässt.
- **Umgebungsvariablen:**
  - Stelle sicher, dass die Umgebungsvariablen `OLLAMA_HOST` und `OLLAMA_ORIGINS` korrekt gesetzt wurden. Du kannst dies im Terminal mit `launchctl getenv OLLAMA_HOST` überprüfen.
- **Protokollierung:**
  - Schaue in die Konsolenausgabe (Logs) von ollama, um etwaige Fehlermeldungen oder Hinweise zur Fehlerursache zu identifizieren.

## Lizenz

Dieses Projekt steht unter der MIT-Lizenz – siehe LICENSE für weitere Details.

## Kontakt

Für Fragen oder Unterstützung wende dich bitte an [deine.email@domain.com](mailto:deine.email@domain.com).
