#!/bin/bash
cd "$(dirname "$0")/IntranetInstitucional_Capas.PresentacionWeb"
pkill -f 'xsp4.exe --nonstop --port 5189' 2>/dev/null
sleep 1
exec xsp4 --nonstop --port 5189
