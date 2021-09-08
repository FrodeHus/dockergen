#!/bin/bash

for i in {0..5}
do
  echo "Updating the correct IPs in nodes.conf on redis-cluster-$i"
  kubectl exec -it -n dockergen redis-cluster-$i -- sh /data/recover-pod.sh $(kubectl get pods -n dockergen -l app=redis-cluster -o jsonpath='{range.items[*]}{.status.podIP} ' )
done

kubectl patch statefulset redis-cluster -n dockergen --patch '{"spec": {"template": {"metadata": {"labels": {"date": "'`date +%s`'" }}}}}'
