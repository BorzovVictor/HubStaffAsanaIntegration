## singleton configurations 
instead of using IOptions<T> uses [singleton object of configuration](https://weblog.west-wind.com/posts/2017/dec/12/easy-configuration-binding-in-aspnet-core-revisited)

## Rate limit
In order to protect against excessive use of the API and ensure availability to all consumers the API is rate limited.

Each authenticated user is allowed to make 1000 requests per hour per app.

Authentication requests (/auth) have a lower limit for security reasons. Only 10 requests are allowed per hour per app.

## cron job examples

every 5 min
 - */5 * * * *


 every day at 00:05:00 UTC
 - 0 5 * * *
 
 every day at 21:05:00 UTC
 - 5 21 * * *
