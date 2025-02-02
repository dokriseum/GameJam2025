## Konfiguration

Um sicherzustellen, dass dein lokales LLM von deinem Spiel erreicht werden kann, müssen bestimmte Umgebungsvariablen gesetzt werden.

### IP-Adresse und Ursprünge anpassen

Setze die folgenden Umgebungsvariablen, damit Ollama auf allen Netzwerkschnittstellen (`0.0.0.0`) lauscht und Verbindungen von allen Ursprüngen erlaubt:

```bash
launchctl setenv OLLAMA_HOST "0.0.0.0"
launchctl setenv OLLAMA_ORIGINS "*"
```

Diese Befehle passen die Einstellungen deines Systems an, sodass das LLM auch über das lokale Netzwerk erreichbar ist.

### **Konfiguration für Windows**

Falls du Windows verwendest, kannst du Umgebungsvariablen über die Eingabeaufforderung (CMD) oder PowerShell setzen:

#### **CMD (Eingabeaufforderung):**

```cmd
setx OLLAMA_HOST "0.0.0.0" /M
setx OLLAMA_ORIGINS "*" /M
```

#### **PowerShell:**

```powershell
[System.Environment]::SetEnvironmentVariable("OLLAMA_HOST", "0.0.0.0", [System.EnvironmentVariableTarget]::Machine)
[System.Environment]::SetEnvironmentVariable("OLLAMA_ORIGINS", "*", [System.EnvironmentVariableTarget]::Machine)
```

Falls du das LLM nach Änderungen nicht erreichst, starte deinen Computer oder dein Terminal neu.

### **Persistente Umgebungsvariablen setzen (macOS/Linux)**

Falls du möchtest, dass diese Variablen auch nach einem Neustart erhalten bleiben, füge sie in deine `~/.bashrc`, `~/.zshrc` oder `~/.profile` Datei hinzu:

```bash
echo 'export OLLAMA_HOST="0.0.0.0"' >> ~/.zshrc
echo 'export OLLAMA_ORIGINS="*"' >> ~/.zshrc
source ~/.zshrc
```

Falls du eine andere Shell nutzt (z. B. `bash`), ersetze `~/.zshrc` mit `~/.bashrc` oder `~/.profile`.

## **Starten des LLM**

Um das LLM zu starten, führe die folgenden Befehle im Terminal aus:

### **1. Bestehende Ollama-Prozesse beenden**

Damit sichergestellt ist, dass keine alten Instanzen laufen, beende alle laufenden Ollama-Prozesse:

```bash
pkill ollama
```

Falls Ollama nicht läuft, wird dieser Befehl keine Auswirkungen haben.

### **2. Lade das LLM mit der angepassten IP-Konfiguration**

Starte Ollama im Serve-Modus und achte darauf, dass die Umgebungsvariable `OLLAMA_HOST` korrekt verwendet wird:

```bash
OLLAMA_HOST=0.0.0.0 ollama serve &
```

Das `&` sorgt dafür, dass der Prozess im Hintergrund läuft und dein Terminal weiterhin verfügbar bleibt. Falls du möchtest, dass Ollama auch nach einem Terminal-Neustart weiterläuft, kannst du `nohup` verwenden:

```bash
nohup ollama serve > ollama.log 2>&1 &
```

### **Starten auf Windows**

Falls du Windows verwendest, starte Ollama mit der CMD oder PowerShell:

#### **CMD:**

```cmd
OLLAMA_HOST=0.0.0.0 ollama serve
```

#### **PowerShell:**

```powershell
$env:OLLAMA_HOST = "0.0.0.0"
ollama serve
```

Falls du die Variablen dauerhaft setzen möchtest, nutze die Umgebungsvariablen-Konfiguration weiter oben.

## **Verbindung mit dem Spiel**

Dein Spiel wird über die IP-Adresse `0.0.0.0` (bzw. die konfigurierte Adresse) auf das LLM zugreifen. Stelle sicher, dass in deinem Spiel die URL korrekt konfiguriert ist, z. B.:

```csharp
string apiUrl = "http://127.0.0.1:11434/api/generate";
```

Falls das LLM auf einem anderen Rechner im Netzwerk läuft, ersetze `127.0.0.1` mit der IP-Adresse des Rechners, auf dem Ollama ausgeführt wird.

## **Fehlerbehebung**

### **1. Verbindungsprobleme**

- Überprüfe, ob Ollama tatsächlich auf `0.0.0.0` lauscht:
  
  ```bash
  netstat -an | grep 11434
  ```

- Falls keine Ausgabe erscheint, überprüfe die Logs oder starte Ollama neu.

- Stelle sicher, dass die Firewall deines Systems Verbindungen auf dem entsprechenden Port (z. B. `11434`) zulässt:
  
  ```bash
  sudo ufw allow 11434/tcp  # Für Linux mit UFW-Firewall
  sudo firewall-cmd --add-port=11434/tcp --permanent && sudo firewall-cmd --reload  # Für CentOS/RHEL
  ```
  
  Falls du unter Windows arbeitest, öffne den Port in den Windows-Firewall-Einstellungen.

### **2. Umgebungsvariablen prüfen**

Falls das LLM nicht wie erwartet startet, überprüfe, ob die Umgebungsvariablen korrekt gesetzt wurden:

```bash
launchctl getenv OLLAMA_HOST
launchctl getenv OLLAMA_ORIGINS
```

Falls sie nicht gesetzt sind, wiederhole die Konfiguration.

### **3. Logs überprüfen**

Falls Ollama nicht reagiert oder Fehler auftreten, überprüfe die Logs:

```bash
tail -f ollama.log
```

Falls du `nohup` verwendet hast, werden alle Ausgaben in `ollama.log` gespeichert.

## **Hilfreiche Ressourcen & Videos**

Falls du mehr über die Einrichtung von Ollama und lokaler KI-Nutzung erfahren möchtest, findest du hier zwei hilfreiche Videoanleitungen:

- **Video 1:** [Ollama Einrichtung & Nutzung](https://www.youtube.com/watch?v=3chfe8Q9rtQ&t=1131s)
- **Video 2:** [Morhpeus lokales KI-Setup](https://www.youtube.com/watch?v=SUYXdI1kC08&t=1062s&pp=ygUYbW9ycGhldXMgbG9rYWwgbnV0emVuIGtp)

## **Lizenz**

Dieses Projekt steht unter der MIT-Lizenz – siehe `LICENSE` für weitere Details.

## **Kontakt**

Für Fragen oder Unterstützung wende dich bitte an [deine.email@domain.com](mailto:deine.email@domain.com).
