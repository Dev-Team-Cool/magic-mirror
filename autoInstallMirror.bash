###MAGIC MIRROR###
cd
git clone  https://github.com/MichMich/MagicMirror
cd MagicMirror/
npm install
cp ~/magic-mirror/modules/config.js config/config.js
cp -r ~/magic-mirror/modules/MMM-Button modules
cp -r ~/magic-mirror/modules/MMM-facialrec modules
cp -r ~/magic-mirror/modules/MMM-GoogleAssistant modules
cp -r ~/magic-mirror/modules/MMM-googleCalendar modules
cp -r ~/magic-mirror/modules/MMM-NMBS-Connection modules
cp -r ~/magic-mirror/modules/MMM-YouTube-API modules
cp -r ~/magic-mirror/modules/MMM-smilerec modules
cp -r ~/magic-mirror/modules/MMM-Page-Selector
npm install ~/MagicMirror/modules/MMM-smilerec
npm install ~/MagicMirror/modules/MMM-facialrec
pip3 install gdown
cd ~/MagicMirror/modules/MMM-GoogleAssistant
git clone https://github.com/Dev-Team-Cool/google-assistant
cd ~/MagicMirror/modules/MMM-facialrec
gdown https://drive.google.com/uc?id=1TpBzbvNWXaprmSeTc0cfUTuPYMXqsfgP 
npm install --prefix  ~/MagicMirror/modules/MMM-GoogleAssistant 
cd ~/MagicMirror/modules
git clone https://github.com/mykle1/MMM-EyeCandy.git
git clone https://github.com/qistoph/MMM-MyCommute.git
npm install --prefix  ~/MagicMirror/modules/MMM-MyCommute
git clone https://github.com/edward-shen/MMM-page-indicator.git
git clone https://github.com/Matzefication/MMM-Hello-Mirror.git
npm install --prefix  ~/MagicMirror/modules/MMM-Hello-Mirror
git clone https://github.com/vicmora/MMM-GoogleMapsTraffic.git
npm install --prefix  ~/MagicMirror/modules/MMM-GoogleMapsTraffic
git clone https://github.com/evghenix/MMM-QRCode.git
git clone https://github.com/Kreshnik/MMM-JokeAPI.git
