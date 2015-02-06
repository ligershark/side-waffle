#
# For information about the Dockerfile format see http://docs.docker.com/reference/builder/
#
# Example taken from http://blogs.msdn.com/b/webdev/archive/2015/01/14/running-asp-net-5-applications-in-linux-containers-with-docker.aspx
#
FROM microsoft/aspnet

COPY . /app
WORKDIR /app
RUN ["kpm", "restore"]

EXPOSE 5004
ENTRYPOINT ["k", "kestrel"]
