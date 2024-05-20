FROM openjdk:22
COPY tcp_ip_pt2.jar /work/secrets.jar
COPY ./songlists /work/songlists
WORKDIR /work
EXPOSE 8080
ENTRYPOINT [ "java","--enable-preview", "-jar","secrets.jar"]