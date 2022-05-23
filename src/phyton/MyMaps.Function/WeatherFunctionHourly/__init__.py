import logging
import os
import requests
import azure.functions as func



subscriptionKey = os.environ["MapsSubscriptionKey"]
hourlyForecastEndpoint = "https://atlas.microsoft.com/weather/forecast/hourly/json?api-version=1.1&query={0}&subscription-key=" + subscriptionKey;

def main(req: func.HttpRequest) -> func.HttpResponse:
    logging.info('Python HTTP trigger function processed a request.')

    coordinates = req.params.get('coordinates')
    response = requests.get(hourlyForecastEndpoint.format(coordinates))
    return func.HttpResponse(
            response.content,
            status_code=200
    )
