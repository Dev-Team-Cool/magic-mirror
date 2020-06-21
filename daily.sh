###Daily start###
python3 ~/magic-mirror/python/train.py
cd  ~/magic-mirror
curl   --header "Content-Type: application/json" --request POST -d @user.json http://localhost:5002​/api​/Auth​/token > dailyKey.json
