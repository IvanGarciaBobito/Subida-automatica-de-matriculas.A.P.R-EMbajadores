# Subida-automatica-de-matriculas.A.P.R-EMbajadores

Programa creado hace unos años para subir las capturas de matriculas de los vehiculos que accedian al negocio al ftp del A.P.R de embajadores.

El programa sube las capturas de matriculas al servidor ftp del A.P.R de embajadores de el dia anterior de su ejecucion,
comprueba si las capturas de ese dia estan ya subidas al ftp y si no las sube y muestra la comparacion de las capturas subidas 
y las que se han subido para observar que se hayan subido correctamente,guarda toda la informacion en un txt a en forma de log.

Este programa funciona en cojunto con unos scripts que hice que a x hora abria el programa fortinet para conectarse al vpn que nos facilitaron,
seguidamente a los x minutos se ejecutaba este programa subiendo las capturas automaticamente al servidor ftp, despues a los x minutos otro script cerraba los programas.

Este programa lo desarrolle por voluntad propia para que las capturas se subieran automaticamente ahorrandonos el estar subiendolas manualmente,que se subieran automaticamente
los dias que el negocio estaba cerrado y asi evitar problemas y que el dia siguiente laboral hubiera que subir bastantes archivos.
