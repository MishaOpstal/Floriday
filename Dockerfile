# Generic container for school projects
FROM node:21
WORKDIR /app
COPY ./app /app

RUN apt-get update
RUN apt-get install -y curl git
RUN npm i bootstrap@5.3.8
