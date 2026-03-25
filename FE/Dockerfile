# Sử dụng Node.js 22 làm base image
FROM node:22 AS build

# Set working directory
WORKDIR /app

# Copy package files
COPY package*.json ./

# Install dependencies
RUN npm install --legacy-peer-deps

# reCAPTCHA site key (public key — safe to hardcode)
ARG REACT_APP_RECAPTCHA_SITE_KEY=6LfnZ5IsAAAAAN47CU2WfoyHVQkU4ID0w2g-9GAE
ENV REACT_APP_RECAPTCHA_SITE_KEY=$REACT_APP_RECAPTCHA_SITE_KEY

# Copy phần còn lại của mã nguồn
COPY . .

# Debug: in ra để xác nhận env var đã được set
RUN echo "RECAPTCHA KEY = $REACT_APP_RECAPTCHA_SITE_KEY"

# Build ứng dụng ReactJS
RUN npm run build

# Sử dụng Nginx để phục vụ ứng dụng
FROM nginx:alpine

# Copy file cấu hình Nginx
COPY nginx/nginx.conf /etc/nginx/conf.d/default.conf

# Copy build ra thư mục chứa các file tĩnh của Nginx
COPY --from=build /app/build /usr/share/nginx/html

# Expose port 80
EXPOSE 80

# Chạy Nginx
CMD ["nginx", "-g", "daemon off;"]
