# Task

Create and implement a backend system that is called from the sample UI that we have provided for you in the index.html file. It must meet the following requirements:

- The user identifier specified in the input field must be passed to the backend
- The backend must return appropriate data to the client so that an image will be displayed
- These rules must be applied in the following order of precedence to determine which data to return to the client:
    - If the last character of the user identifier is [6, 7, 8, 9], retrieve the image URL from `https://my-json-server.typicode.com/ck-pacificdev/tech-test/images/{lastDigitOfUserIdentifier}` where `{lastDigitOfUserIdentifier}` is the last digit of the identifier
    - If the user last character of the user identifier is [1, 2, 3, 4, 5], retrieve the image URL from the `data.db` Sqlite database where the `images.id` value matches the last digit of the identifier
    - If the user identifier contains at least one vowel character (`aeiou`), display the image from `https://api.dicebear.com/8.x/pixel-art/png?seed=vowel&size=150`
    - If the user identifier contains a non-alphanumeric character, pick a random number between 1-5 and display the image with the appropriate seed (e.g. `https://api.dicebear.com/8.x/pixel-art/png?seed={randomNumber}&size=150`)
    - If none of the above conditions are met, display the image from `https://api.dicebear.com/8.x/pixel-art/png?seed=default&size=150`

Once you are happy with your solution, please answer the following questions. There is no need for an essay - bullet pointing the key bits is completely fine!

1. How did you verify that everything works correctly?
2. How long did it take you to complete the task?
3. What else could be done to your solution to make it ready for production?
