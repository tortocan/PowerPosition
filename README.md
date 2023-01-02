# PowerPosition

The docker file attached does the following:
1. Copies your project files across and restores them.
2. Copies your application files across to the docker image.
3. Publishes your application to the out location.
3. Creates a new netcore runtime image.
4. Copies your published app to the new image.
5. Installs cron into the runtime image.
6. Copies scripts and schedule file, removes carrage returns and sets permissions
5. Copies environment variables so that cron can access them.
6. Runs cron and outputs the contents of the /var/log/cron.log file to the screen.

## How to use
To use this within your own dotnet core console app you will need the following files:

* Dockerfile
* schedule
* export_env.sh
* run_app.sh
* .dockerignore 
* docker-compose.yml

If you need to modify the CRON job you need to modify the schedule file this website will parse the CRON https://crontab.guru/.

Then run:

`docker-compose up`
