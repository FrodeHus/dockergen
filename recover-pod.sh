#!/bin/sh
set -e

REDIS_NODES_FILE="/data/nodes.conf"
for redis_node_ip in "$@"
do
  redis_node_id=`redis-cli -h $redis_node_ip -p 6379 cluster nodes | grep myself | awk '{print $1}'`
  sed -i.bak -e "/^$redis_node_id/ s/[0-9]\{1,3\}\.[0-9]\{1,3\}\.[0-9]\{1,3\}\.[0-9]\{1,3\}/${redis_node_ip}/" ${REDIS_NODES_FILE}
done
